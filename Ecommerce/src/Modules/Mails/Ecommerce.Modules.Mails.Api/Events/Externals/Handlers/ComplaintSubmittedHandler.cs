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
    internal class ComplaintSubmittedHandler : IEventHandler<ComplaintSubmitted>
    {
        private readonly IMailService _mailService;
        private readonly CompanyOptions _companyOptions;
        private const string _mailTemplatePath = "MailTemplates\\MailTemplate.html";

        public ComplaintSubmittedHandler(IMailService mailService, CompanyOptions companyOptions)
        {
            _mailService = mailService;
            _companyOptions = companyOptions;
        }
        public async Task HandleAsync(ComplaintSubmitted @event)
        {
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Complaint submitted");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", @event.FirstName);
            bodyHtml = bodyHtml.Replace("{message}", $"Thank you for reaching out to us and bringing your concerns to our attention. We have received your complaint related to your order: {@event.OrderId}. " +
                $"We aim to resolve your issue as quickly as possible, and you can expect a response from us within 7 working days.If you have any questions or would like to provide further details, please feel free to contact us.");
            await _mailService.SendAsync(new MailSendDto()
            {
                To = @event.Email,
                Subject = $"Order confirmation ID: {@event.OrderId}",
                Body = bodyHtml,
                OrderId = @event.OrderId,
                CustomerId = @event.CustomerId
            });
        }
    }
}
