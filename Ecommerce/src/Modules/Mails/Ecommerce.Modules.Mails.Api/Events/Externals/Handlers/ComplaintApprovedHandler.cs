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
    internal class ComplaintApprovedHandler : IEventHandler<ComplaintApproved>
    {
        private readonly IMailService _mailService;
        private readonly CompanyOptions _companyOptions;
        private const string _mailTemplatePath = "MailTemplates\\MailTemplate.html";

        public ComplaintApprovedHandler(IMailService mailService, CompanyOptions companyOptions)
        {
            _mailService = mailService;
            _companyOptions = companyOptions;
        }
        public async Task HandleAsync(ComplaintApproved @event)
        {
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Complaint submitted");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", @event.FirstName);
            bodyHtml = bodyHtml.Replace("{message}", $"Thank you for your patience while we reviewed your complaint {@event.ComplaintId} regarding {@event.Title} related to your order {@event.OrderId}. " +
                $"After a thorough investigation, we are pleased to inform you that your complaint has been approved. \n" +
                $"{@event.Decision} \n" +
                $"Refunded amount: {@event.RefundedAmount ?? 0} (optional)" +
                $"{@event.AdditionalInformation} \n" +
                $"We sincerely apologize for any inconvenience this matter may have caused and are committed to making things right. Should you have any further questions or concerns, please do not hesitate to reach out to us.");
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
