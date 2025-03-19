using Ecommerce.Modules.Mails.Api.DTO;
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
    internal class ComplaintApprovedHandler : IEventHandler<ComplaintApproved>
    {
        private readonly IMailService _mailService;
        private readonly CompanyOptions _companyOptions;
        private readonly StripeOptions _stripeOptions;
        private const string _mailTemplatePath = "MailTemplates\\ComplaintApproved.html";

        public ComplaintApprovedHandler(IMailService mailService, CompanyOptions companyOptions, StripeOptions stripeOptions)
        {
            _mailService = mailService;
            _companyOptions = companyOptions;
            _stripeOptions = stripeOptions;
        }
        public async Task HandleAsync(ComplaintApproved @event)
        {
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Complaint has been submitted");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", @event.FirstName);
            bodyHtml = bodyHtml.Replace("{complaintId}", @event.ComplaintId.ToString());
            bodyHtml = bodyHtml.Replace("{title}", @event.Title);
            bodyHtml = bodyHtml.Replace("{orderId}", @event.OrderId.ToString());
            bodyHtml = bodyHtml.Replace("{decision}", @event.Decision);
            bodyHtml = bodyHtml.Replace("{additionalInformation}", @event.AdditionalInformation ?? "0");
            bodyHtml = bodyHtml.Replace("{refundedAmount}", @event.RefundedAmount.ToString());
            bodyHtml = bodyHtml.Replace("{currency}", _stripeOptions.Currency);
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
