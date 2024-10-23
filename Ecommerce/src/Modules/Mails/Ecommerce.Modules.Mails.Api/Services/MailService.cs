using Ecommerce.Modules.Mails.Api.DAL;
using Ecommerce.Modules.Mails.Api.DAL.Mappings;
using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Modules.Mails.Api.Entities;
using Ecommerce.Modules.Mails.Api.Exceptions;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Infrastructure.Mails;
using Ecommerce.Shared.Infrastructure.Pagination;
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
        private readonly IBlobStorageService _blobStorageService;
        private readonly TimeProvider _timeProvider;

        public MailService(IMailsDbContext dbContext, MailOptions mailOptions, IBlobStorageService blobStorageService, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _mailOptions = mailOptions;
            _blobStorageService = blobStorageService;
            _timeProvider = timeProvider;
        }

        public async Task<CursorPagedResult<MailBrowseDto, MailCursorDto>> BrowseAsync(MailCursorDto cursorDto, bool? isNextPage, int pageSize)
        {
            var mailsAsQueryable = _dbContext.Mails.OrderBy(m => m.Id).AsQueryable();
            int takeAmount = pageSize + 1;
            if (cursorDto is not null)
            {
                if (isNextPage is true)
                {
                    mailsAsQueryable = mailsAsQueryable.Where(m => m.CreatedAt >= cursorDto.CursorCreatedAt && m.Id != cursorDto.CursorId);
                }
                else
                {
                    mailsAsQueryable = mailsAsQueryable.Where(m => m.CreatedAt <= cursorDto.CursorCreatedAt && m.Id != cursorDto.CursorId);
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
                || (cursorDto is not null && mails.First().Id == _dbContext.Mails.OrderBy(m => m.Id).AsNoTracking().First().Id);
            bool hasNextPage = mails.Count > pageSize
                || (cursorDto is not null && isNextPage == false);
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
                await _dbContext.Mails.AddAsync(new Mail(_mailOptions.Email, dto.To, dto.Subject, dto.Body, customer, dto.OrderId, _timeProvider.GetUtcNow().UtcDateTime));
            }
            else
            {
                await _dbContext.Mails.AddAsync(new Mail(_mailOptions.Email, dto.To, dto.Subject, dto.Body, dto.OrderId, _timeProvider.GetUtcNow().UtcDateTime));
            }
            await _dbContext.SaveChangesAsync();
        }
        private async Task<Customer?> GetCustomerAsync(Guid? customerId)
        {
            if(customerId is null || customerId == Guid.Empty) return null;
            var customer = await _dbContext.Customers
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == customerId) ?? throw new CustomerNotFoundException((Guid)customerId);
            return customer;
        }
    }
}
