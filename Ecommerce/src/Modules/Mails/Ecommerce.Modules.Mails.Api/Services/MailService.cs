using Ecommerce.Modules.Mails.Api.DAL;
using Ecommerce.Modules.Mails.Api.DAL.Mappings;
using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Modules.Mails.Api.Entities;
using Ecommerce.Modules.Mails.Api.Exceptions;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Infrastructure.Company;
using Ecommerce.Shared.Infrastructure.Mails;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Services;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
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
        //private readonly IBlobStorageService _blobStorageService;
        private readonly CompanyOptions _companyOptions;
        private readonly IFilterService _filterService;
        private const string _mailDefaultTemplatePath = "MailTemplates\\Default.html";

        public MailService(IMailsDbContext dbContext, MailOptions mailOptions/*, IBlobStorageService blobStorageService*/, 
            CompanyOptions companyOptions, IFilterService filterService)
        {
            _dbContext = dbContext;
            _mailOptions = mailOptions;
            //_blobStorageService = blobStorageService;
            _companyOptions = companyOptions;
            _filterService = filterService;
        }

        public async Task<CursorPagedResult<MailBrowseDto, MailCursorDto>> BrowseAsync(MailCursorDto cursorDto, bool? isNextPage, int pageSize)
        {
            var mailsAsQueryable = _dbContext.Mails
                .Include(m => m.Customer)
                .OrderByDescending(m => m.CreatedAt)
                .ThenBy(m => m.Id)
                .AsQueryable();
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
                .ToListAsync();
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

        public async Task<MailDetailsDto> GetAsync(int mailId)
        {
            var mail = await _dbContext.Mails.SingleOrDefaultAsync(m => m.Id == mailId) ?? throw new MailNotFoundException(mailId);
            return mail.AsDetailsDto();
        }

        public async Task SendAsync(MailSendDto dto)
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
                foreach(var file in dto.Files)
                {
                    await file.CopyToAsync(stream);
                    multipart.Add(new MimePart(file.ContentType)
                    {
                        Content = new MimeContent(stream),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = file.FileName
                    });
                }
            }
            multipart.Add(bodyHtml);
            email.Body = multipart;
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailOptions.SmtpHost, _mailOptions.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailOptions.Email, _mailOptions.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            await stream.DisposeAsync();
            if(customer is not null)
            {
                await _dbContext.Mails.AddAsync(new Mail(_mailOptions.Email, dto.To, dto.Subject, dto.Body, customer, dto.OrderId));
            }
            else
            {
                await _dbContext.Mails.AddAsync(new Mail(_mailOptions.Email, dto.To, dto.Subject, dto.Body, dto.OrderId));
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task SendAsync(MailSendDefaultBodyDto dto)
        {
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
            });
        }

        private async Task<Customer?> GetCustomerAsync(Guid? customerId)
        {
            if(customerId is null || customerId == Guid.Empty) return null;
            var customer = await _dbContext.Customers
                //.AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == customerId) ?? throw new CustomerNotFoundException((Guid)customerId);
            return customer;
        }
    }
}
