using Ecommerce.Modules.Mails.Api.DAL;
using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Modules.Mails.Api.Exceptions;
using Ecommerce.Modules.Mails.Api.Services;
using Ecommerce.Shared.Abstractions.Events;
using Ecommerce.Shared.Infrastructure.Company;
using Ecommerce.Shared.Infrastructure.Stripe;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Events.Externals.Handlers
{
    internal class OfferAcceptedHandler : IEventHandler<OfferAccepted>
    {
        private readonly IMailService _mailService;
        private readonly IMailsDbContext _dbContext;
        private readonly CompanyOptions _companyOptions;
        private readonly StripeOptions _stripeOptions;
        private const string _mailTemplatePath = "MailTemplates\\OfferAccepted.html";

        public OfferAcceptedHandler(IMailService mailService, IMailsDbContext dbContext, CompanyOptions companyOptions, StripeOptions stripeOptions)
        {
            _mailService = mailService;
            _dbContext = dbContext;
            _companyOptions = companyOptions;
            _stripeOptions = stripeOptions;
        }
        public async Task HandleAsync(OfferAccepted @event)
        {
            var customer = await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == @event.CustomerId) ?? throw new CustomerNotFoundException(@event.CustomerId);
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Offer has been accepted");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", customer.FirstName);
            bodyHtml = bodyHtml.Replace("{offerId}", @event.OfferId.ToString());
            bodyHtml = bodyHtml.Replace("{currency}", _stripeOptions.Currency);
            bodyHtml = bodyHtml.Replace("{productName}", @event.ProductName);
            bodyHtml = bodyHtml.Replace("{SKU}", @event.SKU);
            bodyHtml = bodyHtml.Replace("{oldPrice}", @event.OldPrice.ToString());
            bodyHtml = bodyHtml.Replace("{offeredPrice}", @event.OfferedPrice.ToString());
            bodyHtml = bodyHtml.Replace("{code}", @event.Code);
            bodyHtml = bodyHtml.Replace("{expiresAt}", @event.ExpiresAt.ToString("dd/MM/yyyy"));
            await _mailService.SendAsync(new MailSendDto()
            {
                To = customer.Email,
                Subject = $"Offer has been accepted for ID: {@event.OfferId}",
                Body = bodyHtml,
                CustomerId = customer.Id
            });
        }
    }
}
