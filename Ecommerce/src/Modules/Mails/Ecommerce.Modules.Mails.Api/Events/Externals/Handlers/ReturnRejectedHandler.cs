using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Modules.Mails.Api.Entities.ValueObjects;
using Ecommerce.Modules.Mails.Api.Services;
using Ecommerce.Shared.Abstractions.Events;
using Ecommerce.Shared.Infrastructure.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Events.Externals.Handlers
{
    internal class ReturnRejectedHandler : IEventHandler<ReturnRejected>
    {
        private readonly IMailService _mailService;
        private readonly CompanyOptions _companyOptions;
        private const string _mailTemplatePath = "MailTemplates\\MailTemplateWithProductTable.html";

        public ReturnRejectedHandler(IMailService mailService, CompanyOptions companyOptions)
        {
            _mailService = mailService;
            _companyOptions = companyOptions;
        }
        public async Task HandleAsync(ReturnRejected @event)
        {
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Return rejected for ID: {@event.ReturnId}");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", @event.FirstName);
            bodyHtml = bodyHtml.Replace("{message}", $"Thank you for reaching out to us regarding the return: {@event.ReturnId} that was submitted on {@event.CreatedAt} of your order: {@event.OrderId}. " +
                $"After reviewing your return request, we regret to inform you that we are unable to process the return at this time due to: {@event.RejectReason}." +
                $"We understand this may be disappointing, and we want to ensure clarity regarding our return policy. For more information feel free to contact us.");
            bodyHtml = bodyHtml.Replace("{items}", GenerateItemsHtml(@event.Products));
            await _mailService.SendAsync(new MailSendDto()
            {
                To = @event.Email,
                Subject = $"Order confirmation ID: {@event.OrderId}",
                Body = bodyHtml,
                OrderId = @event.OrderId,
                CustomerId = @event.CustomerId
            });
        }
        private string GenerateItemsHtml(IEnumerable<Product> Products)
        {
            StringBuilder productsHtml = new StringBuilder();
            foreach (var product in Products)
            {
                productsHtml.Append($"""
                    <tr>
                        <td>{product.Name}</td>
                        <td>{product.SKU}</td>
                        <td>{product.Price}</td>
                        <td>{product.Quantity}</td>
                    </tr>
                    """);
            }
            return productsHtml.ToString();
        }
    }
}
