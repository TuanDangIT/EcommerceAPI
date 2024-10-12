using Ecommerce.Modules.Mails.Api.Services;
using Ecommerce.Shared.Abstractions.Events;
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

        public OfferRequestedHandler(IMailService mailService)
        {
            _mailService = mailService;
        }
        public Task HandleAsync(OfferRequested @event)
        {
            throw new NotImplementedException();
        }
    }
}
