using Ecommerce.Modules.Mails.Api.DAL;
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
    internal class OrderCreatedHandler : IEventHandler<OrderCreated>
    {
        private readonly IMailService _mailService;
        private readonly CompanyOptions _companyOptions;
        private const string _mailTemplatePath = "MailTemplates\\MailTemplateWithProductTableAndTotalPrice.html";

        public OrderCreatedHandler(IMailService mailService, CompanyOptions companyOptions)
        {
            _mailService = mailService;
            _companyOptions = companyOptions;
        }
        public async Task HandleAsync(OrderCreated @event)
        {
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Order confirmation for ID: {@event.OrderId}");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", @event.FirstName);
            bodyHtml = bodyHtml.Replace("{message}", $"Thank you for your order! We are pleased to confirm that we have received your order {@event.OrderId}, placed on {@event.PlacedAt}. " +
                $"Should you have any questions or require further assistance, feel free to contact us.");
            bodyHtml = bodyHtml.Replace("{totalPrice}", @event.TotalSum.ToString());
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
