using Ecommerce.Modules.Mails.Api.DAL;
using Ecommerce.Modules.Mails.Api.DAL.Mappings;
using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Modules.Mails.Api.Entities;
using Ecommerce.Modules.Mails.Api.Exceptions;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.Events;
using Ecommerce.Shared.Infrastructure.Company;
using Ecommerce.Shared.Infrastructure.Mails;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Services;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ecommerce.Modules.Mails.Api.Services
{
    internal class MailService : IMailService
    {
        private readonly IMailsDbContext _dbContext;
        private readonly MailOptions _mailOptions;
        private readonly IBlobStorageService _blobStorageService;
        private readonly CompanyOptions _companyOptions;
        private readonly IFilterService _filterService;
        private readonly ILogger<MailService> _logger;
        private readonly IContextService _contextService;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly IOptions<SieveOptions> _sieveOptions;
        private const string _mailDefaultTemplatePath = "MailTemplates/Default.html";
        private const string _containerName = "mails";

        public MailService(IMailsDbContext dbContext, MailOptions mailOptions, IBlobStorageService blobStorageService, 
            CompanyOptions companyOptions, IFilterService filterService, ILogger<MailService> logger, IContextService contextService,
            [FromKeyedServices("mails-sieve-processor")] ISieveProcessor sieveProcessor, IOptions<SieveOptions> sieveOptions)
        {
            _dbContext = dbContext;
            _mailOptions = mailOptions;
            _blobStorageService = blobStorageService;
            _companyOptions = companyOptions;
            _filterService = filterService;
            _logger = logger;
            _contextService = contextService;
            _sieveProcessor = sieveProcessor;
            _sieveOptions = sieveOptions;
        }

        public async Task<CursorPagedResult<MailBrowseDto, MailCursorDto>> BrowseCursorAsync(MailCursorDto cursorDto, CancellationToken cancellationToken = default)
        {
            var mailsAsQueryable = _dbContext.Mails
                .Include(m => m.Customer)
                .Include(m => m.AttachmentFiles)
                .OrderByDescending(m => m.CreatedAt)
                .ThenBy(m => m.Id)
                .AsQueryable();
            var pageSize = cursorDto.PageSize;
            var isNextPage = cursorDto.IsNextPage;
            if (cursorDto.Filters is not null && cursorDto.Filters.Count != 0)
            {
                foreach (var filter in cursorDto.Filters)
                {
                    mailsAsQueryable = _filterService.ApplyFilter(mailsAsQueryable, filter.Key, filter.Value);
                }
            }
            int takeAmount = pageSize + 1;
            if (cursorDto is not null)
            {
                if (isNextPage is true)
                {
                    mailsAsQueryable = mailsAsQueryable.Where(m => m.CreatedAt <= cursorDto.CursorCreatedAt && m.Id != cursorDto.CursorId);
                }
                else
                {
                    mailsAsQueryable = mailsAsQueryable.Where(m => m.CreatedAt >= cursorDto.CursorCreatedAt && m.Id != cursorDto.CursorId);
                    takeAmount = pageSize;
                }
            }
            mailsAsQueryable = mailsAsQueryable.Take(takeAmount);
            if (isNextPage is false && cursorDto is not null)
            {
                mailsAsQueryable = mailsAsQueryable.Reverse();
            }
            var mails = await mailsAsQueryable
                .Select(m => m.AsBrowseDto())
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            bool isFirstPage = cursorDto is null
                || (cursorDto is not null && mails.First().Id == _dbContext.Mails.OrderByDescending(m => m.CreatedAt)
                    .ThenBy(m => m.Id).AsNoTracking().First().Id);
            bool hasNextPage = mails.Count > pageSize
                || (cursorDto is not null && isNextPage == false);
            if (mails.Count >pageSize)
            {
                mails.RemoveAt(mails.Count - 1);
            }
            MailCursorDto nextCursor = hasNextPage ?
                new MailCursorDto()
                {
                    CursorId = mails.Last().Id,
                    CursorCreatedAt = mails.Last().CreatedAt
                }
                : new();
            MailCursorDto previousCursor = mails.Count > 0 ?
                new MailCursorDto()
                {
                    CursorId = mails.First().Id,
                    CursorCreatedAt = mails.First().CreatedAt
                }
                : new();
            return new CursorPagedResult<MailBrowseDto, MailCursorDto>(mails, nextCursor, previousCursor, isFirstPage);
        }

        public async Task<PagedResult<MailBrowseDto>> BrowseOffsetAsync(SieveModel model, CancellationToken cancellationToken = default)
        {
            if (model.Page is null || model.Page <= 0)
            {
                throw new PaginationException();
            }
            var mails = _dbContext.Mails
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, mails)
                .Select(m => m.AsBrowseDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(model, mails, applyPagination: false, applySorting: false)
                .CountAsync(cancellationToken);
            int pageSize = _sieveOptions.Value.DefaultPageSize;
            if (model.PageSize is not null || model.PageSize <= 0)
            {
                pageSize = model.PageSize.Value;
            }
            var pagedResult = new PagedResult<MailBrowseDto>(dtos, totalCount, pageSize, model.Page.Value);
            return pagedResult;
        }

        public async Task<MailDetailsDto> GetAsync(int mailId, CancellationToken cancellationToken = default)
            => await _dbContext.Mails
                .Where(m => m.Id == mailId)
                .Select(m => m.AsDetailsDto())
                .FirstOrDefaultAsync(cancellationToken) ??
                throw new MailNotFoundException(mailId);

        public async Task SendAsync(MailSendDto dto)
        {
            List<AttachmentFile> files = [];
            try
            {
                var customer = await GetCustomerAsync(dto.CustomerId);
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_mailOptions.Email));
                email.To.Add(MailboxAddress.Parse(dto.To));
                email.Subject = dto.Subject;
                var stream = new MemoryStream();
                var bodyHtml = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = dto.Body
                };
                var multipart = new Multipart("mixed");
                if (dto.Files is not null && dto.Files.Any())
                {
                    foreach (var file in dto.Files)
                    {
                        await file.CopyToAsync(stream);
                        multipart.Add(new MimePart(file.ContentType)
                        {
                            Content = new MimeContent(stream),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = file.FileName
                        });
                        var fileId = Ulid.NewUlid();
                        var uniqueBlobFileName = GenerateUniqueBlobFileNameForAttachments(file.FileName, fileId);
                        await _blobStorageService.UploadAsync(file, uniqueBlobFileName, _containerName);
                        files.Add(new AttachmentFile(fileId, file.FileName));
                    }
                }
                if (customer is not null)
                {
                    await _dbContext.Mails.AddAsync(new Mail(_mailOptions.Email, dto.To, dto.Subject, dto.Body, customer, dto.OrderId, files));
                }
                else
                {
                    await _dbContext.Mails.AddAsync(new Mail(_mailOptions.Email, dto.To, dto.Subject, dto.Body, dto.OrderId, files));
                }
                await _dbContext.SaveChangesAsync();
                multipart.Add(bodyHtml);
                email.Body = multipart;
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_mailOptions.SmtpHost, _mailOptions.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailOptions.Email, _mailOptions.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                await stream.DisposeAsync();
            }
            catch
            {
                if (!files.IsNullOrEmpty())
                {
                    await _blobStorageService.DeleteManyAsync(files.Select(f => GenerateUniqueBlobFileNameForAttachments(f.FileName, f.Id)), _containerName);
                }
                throw;
            }
        }

        public async Task SendAsync(MailSendDefaultBodyDto dto)
        {
            //if (dto.CustomerId is not null)
            //{
            //    var exists = await _dbContext.Customers
            //        .AsNoTracking()
            //        .AnyAsync(c => c.Id == dto.CustomerId);

            //    if(exists is false)
            //    {
            //        throw new CustomerNotFoundException((Guid)dto.CustomerId);
            //    }
            //}
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailDefaultTemplatePath));
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", dto.FirstName ?? "customer");
            bodyHtml = bodyHtml.Replace("{message}", dto.Message);
            await SendAsync(new MailSendDto()
            {
                To = dto.To,
                Subject = dto.Subject,
                Body = bodyHtml,
                OrderId = dto.OrderId,
                CustomerId = dto.CustomerId,
                Files = dto.Files
            });
            _logger.LogInformation("Mail: {@mail} was sent by {@user}.", dto, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }

        public async Task<FileStreamResult> DownloadAttachmentFileAsync(int mailId, string stringdFileId)
        {
            if (!Ulid.TryParse(stringdFileId, out var fileId))
            {
                throw new UlidCannotParseException(stringdFileId);
            }
            var mail = await _dbContext.Mails
                .Include(m => m.AttachmentFiles)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == mailId) ?? throw new MailNotFoundException(mailId);
            if (mail.AttachmentFiles.IsNullOrEmpty())
            {
                throw new AttachmentFileNotFoundException(mailId, fileId);
            }
            var attachmentFile = mail.AttachmentFiles!
                .FirstOrDefault(af => af.Id == fileId) ?? throw new AttachmentFileNotFoundException(mailId, fileId);
            var blobFileName = GenerateUniqueBlobFileNameForAttachments(attachmentFile.FileName, fileId);
            var file = await _blobStorageService.DownloadAsync(blobFileName, _containerName);
            var fileResult = new FileStreamResult(file.FileStream, file.ContentType)
            {
                FileDownloadName = attachmentFile.FileName
            };
            return fileResult;
        }

        private async Task<Customer?> GetCustomerAsync(Guid? customerId)
        {
            if(customerId is null || customerId == Guid.Empty) return null;
            var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId);
            return customer;
        }

        private string GenerateUniqueBlobFileNameForAttachments(string fileName, Ulid fileId)
            => fileName + $"-{fileId}";
    }
}
