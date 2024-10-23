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
    internal class OfferRequestedHandler : IEventHandler<OfferRequested>
    {
        private readonly IMailService _mailService;
        private readonly IMailsDbContext _dbContext;
        private readonly CompanyOptions _companyOptions;
        private readonly StripeOptions _stripeOptions;
        private const string _mailTemplatePath = "MailTemplates\\MailTemplate.html";

        public OfferRequestedHandler(IMailService mailService, IMailsDbContext dbContext, CompanyOptions companyOptions, StripeOptions stripeOptions)
        {
            _mailService = mailService;
            _dbContext = dbContext;
            _companyOptions = companyOptions;
            _stripeOptions = stripeOptions;
        }
        public async Task HandleAsync(OfferRequested @event)
        {
            var customer = await _dbContext.Customers
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == @event.CustomerId) ?? throw new CustomerNotFoundException(@event.CustomerId);
            var bodyHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _mailTemplatePath));
            bodyHtml = bodyHtml.Replace("{title}", "Offer requested");
            bodyHtml = bodyHtml.Replace("{companyName}", _companyOptions.Name);
            bodyHtml = bodyHtml.Replace("{customerFirstName}", customer.FirstName);
            bodyHtml = bodyHtml.Replace("{message}", $"Thank you for submitting your offer regarding {@event.ProductName}, {@event.SKU} for {@event.OfferedPrice} {_stripeOptions.Currency} from {@event.OldPrice} {_stripeOptions.Currency}. " +
                $"Please be assured that your offer is being given thorough consideration. We will get back to you with our response in the near future. " +
                $"Should you have any questions or require further assistance, feel free to contact us");
            await _mailService.SendAsync(new MailSendDto()
            {
                To = customer.Email,
                Subject = $"Offer requested",
                Body = bodyHtml,
                CustomerId = @event.CustomerId
            });
        }
    }
}
