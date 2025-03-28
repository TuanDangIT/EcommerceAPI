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
    internal class ComplaintRejectedHandler : IEventHandler<ComplaintRejected>
    {
        private readonly IMailService _mailService;
        private readonly CompanyOptions _companyOptions;
        private const string _mailTemplatePath = "MailTemplates/ComplaintRejected.html";

        public ComplaintRejectedHandler(IMailService mailService, CompanyOptions companyOptions)
        {
            _mailService = mailService;
            _companyOptions = companyOptions;
        }
        public async Task HandleAsync(ComplaintRejected @event)
        {
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Complaint has been rejected");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", @event.FirstName);
            bodyHtml = bodyHtml.Replace("{complaintId}", @event.ComplaintId.ToString());
            bodyHtml = bodyHtml.Replace("{title}", @event.Title);
            bodyHtml = bodyHtml.Replace("{orderId}", @event.OrderId.ToString());
            bodyHtml = bodyHtml.Replace("{decision}", @event.Decision);
            bodyHtml = bodyHtml.Replace("{additionalInformation}", @event.AdditionalInformation);
            await _mailService.SendAsync(new MailSendDto()
            {
                To = @event.Email,
                Subject = $"Complaint has been rejected for order ID: {@event.ComplaintId}",
                Body = bodyHtml,
                OrderId = @event.OrderId,
                CustomerId = @event.CustomerId
            });
        }
    }
}
