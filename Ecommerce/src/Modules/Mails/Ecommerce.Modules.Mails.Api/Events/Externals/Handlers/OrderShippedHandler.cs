﻿using Ecommerce.Modules.Mails.Api.DTO;
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
    internal class OrderShippedHandler : IEventHandler<OrderShipped>
    {
        private readonly IMailService _mailService;
        private readonly CompanyOptions _companyOptions;
        private const string _mailTemplatePath = "MailTemplates/OrderShipped.html";

        public OrderShippedHandler(IMailService mailService, CompanyOptions companyOptions)
        {
            _mailService = mailService;   
            _companyOptions = companyOptions;
        }
        public async Task HandleAsync(OrderShipped @event)
        {
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Order has been shipped");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", @event.FirstName);
            bodyHtml = bodyHtml.Replace("{orderId}", @event.OrderId.ToString());
            bodyHtml = bodyHtml.Replace("{placedAt}", @event.PlacedAt.ToString("dd/MM/yyyy"));
            await _mailService.SendAsync(new MailSendDto()
            {
                To = @event.Email,
                Subject = $"Order has been shipped for ID: {@event.OrderId}",
                Body = bodyHtml,
                OrderId = @event.OrderId,
                CustomerId = @event.CustomerId
            });
        }
    }
}
