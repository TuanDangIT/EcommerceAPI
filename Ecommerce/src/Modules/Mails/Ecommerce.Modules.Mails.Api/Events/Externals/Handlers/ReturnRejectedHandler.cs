﻿using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Modules.Mails.Api.Services;
using Ecommerce.Shared.Abstractions.Events;
using Ecommerce.Shared.Infrastructure.Company;
using Ecommerce.Shared.Infrastructure.Stripe;
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
        private readonly StripeOptions _stripeOptions;
        private const string _mailTemplatePath = "MailTemplates/ReturnRejected.html";

        public ReturnRejectedHandler(IMailService mailService, CompanyOptions companyOptions, StripeOptions stripeOptions)
        {
            _mailService = mailService;
            _companyOptions = companyOptions;
            _stripeOptions = stripeOptions;
        }
        public async Task HandleAsync(ReturnRejected @event)
        {
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Return has been rejected");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", @event.FirstName);
            bodyHtml = bodyHtml.Replace("{returnId}", @event.ReturnId.ToString());
            bodyHtml = bodyHtml.Replace("{orderId}", @event.OrderId.ToString());
            bodyHtml = bodyHtml.Replace("{createdAt}", @event.CreatedAt.ToString("dd/MM/yyyy"));
            bodyHtml = bodyHtml.Replace("{rejectReason}", @event.RejectReason);
            bodyHtml = bodyHtml.Replace("{items}", GenerateItemsHtml(@event.Products));
            await _mailService.SendAsync(new MailSendDto()
            {
                To = @event.Email,
                Subject = $"Return has been rejected for ID: {@event.ReturnId}",
                Body = bodyHtml,
                OrderId = @event.OrderId,
                CustomerId = @event.CustomerId
            });
        }
        private string GenerateItemsHtml(IEnumerable<ProductDto> Products)
        {
            StringBuilder productsHtml = new StringBuilder();
            foreach (var product in Products)
            {
                productsHtml.Append($"""
                    <tr>
                        <td>{product.Name}</td>
                        <td>{product.SKU}</td>
                        <td>{product.Price} {_stripeOptions.Currency}</td>
                        <td>{product.Quantity}</td>
                    </tr>
                    """);
            }
            return productsHtml.ToString();
        }
    }
}
