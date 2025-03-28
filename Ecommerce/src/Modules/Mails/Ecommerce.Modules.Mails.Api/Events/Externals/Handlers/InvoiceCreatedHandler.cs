using Ecommerce.Modules.Mails.Api.DAL;
using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Modules.Mails.Api.Services;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.Events;
using Ecommerce.Shared.Infrastructure.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Events.Externals.Handlers
{
    internal class InvoiceCreatedHandler : IEventHandler<InvoiceCreated>
    {
        private readonly IMailService _mailService;
        private readonly CompanyOptions _companyOptions;
        private readonly IMailsDbContext _dbContext;
        private readonly IBlobStorageService _blobStorageService;
        private const string _mailTemplatePath = "MailTemplates/InvoiceCreated.html";
        private const string _containerName = "invoices";
        private const string _contentType = "application/pdf";

        public InvoiceCreatedHandler(IMailService mailService, CompanyOptions companyOptions, IMailsDbContext dbContext, IBlobStorageService blobStorageService)
        {
            _mailService = mailService;
            _companyOptions = companyOptions;
            _dbContext = dbContext;
            _blobStorageService = blobStorageService;
        }
        public async Task HandleAsync(InvoiceCreated @event)
        {
            var email = _dbContext.Customers
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Email == @event.Email);
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Invoice for order ID: {@event.OrderId}");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", @event.FirstName);
            bodyHtml = bodyHtml.Replace("{orderId}", @event.OrderId.ToString());
            var invoice = await _blobStorageService.DownloadAsync(@event.InvoiceNo, _containerName);
            var stream = invoice.FileStream;
            var file = new FormFile(stream, 0, stream.Length, "invoice", invoice.FileName + "-invoice") 
            { 
                Headers = new HeaderDictionary()
            };
            file.ContentType = _contentType;
            await _mailService.SendAsync(new MailSendDto()
            {
                To = @event.Email,
                Subject = $"Invoice for order ID: {@event.OrderId}",
                Body = bodyHtml,
                OrderId = @event.OrderId,
                Files = new List<IFormFile>()
                {
                    file
                },
                CustomerId = @event.CustomerId
            });
        }
    }
}
