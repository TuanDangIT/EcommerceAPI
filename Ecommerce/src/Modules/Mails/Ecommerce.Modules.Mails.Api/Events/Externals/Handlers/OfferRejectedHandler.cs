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
    internal class OfferRejectedHandler : IEventHandler<OfferRejected>
    {
        private readonly IMailService _mailService;
        private readonly IMailsDbContext _dbContext;
        private readonly CompanyOptions _companyOptions;
        private readonly StripeOptions _stripeOptions;
        private const string _mailTemplatePath = "MailTemplates\\MailTemplate.html";

        public OfferRejectedHandler(IMailService mailService, IMailsDbContext dbContext, CompanyOptions companyOptions, StripeOptions stripeOptions)
        {
            _mailService = mailService;
            _dbContext = dbContext;
            _companyOptions = companyOptions;
            _stripeOptions = stripeOptions;
        }
        public async Task HandleAsync(OfferRejected @event)
        {
            var customer = await _dbContext.Customers
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == @event.CustomerId) ?? throw new CustomerNotFoundException(@event.CustomerId);
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", $"Offer rejected for ID: {@event.OfferId}");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", customer.FirstName);
            bodyHtml = bodyHtml.Replace("{message}", $"Thank you for submitting your offer: {@event.OfferId} regarding {@event.ProductName}, {@event.SKU} for {@event.OfferedPrice} {_stripeOptions.Currency} from {@event.OldPrice} {_stripeOptions.Currency}." +
                $"After careful consideration, we regret to inform you that we rejected your offer. Should you have any questions or require further assistance, feel free to contact us");
            await _mailService.SendAsync(new MailSendDto()
            {
                To = customer.Email,
                Subject = $"Offer rejected for ID: {@event.OfferId}",
                Body = bodyHtml,
                CustomerId = @event.CustomerId
            });
        }
    }
}
