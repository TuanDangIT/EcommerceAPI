using Ecommerce.Modules.Mails.Api.DTO;
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
        private const string _mailTemplatePath = "MailTemplates/ComplaintSubmitted.html";

        public ComplaintSubmittedHandler(IMailService mailService, CompanyOptions companyOptions)
        {
            _mailService = mailService;
            _companyOptions = companyOptions;
        }
        public async Task HandleAsync(ComplaintSubmitted @event)
        {
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Complaint has been submitted");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", @event.FirstName);
            bodyHtml = bodyHtml.Replace("{orderId}", @event.OrderId.ToString());
            await _mailService.SendAsync(new MailSendDto()
            {
                To = @event.Email,
                Subject = $"Complaint has been submitted for order ID: {@event.OrderId}",
                Body = bodyHtml,
                OrderId = @event.OrderId,
                CustomerId = @event.CustomerId
            });
        }
    }
}
