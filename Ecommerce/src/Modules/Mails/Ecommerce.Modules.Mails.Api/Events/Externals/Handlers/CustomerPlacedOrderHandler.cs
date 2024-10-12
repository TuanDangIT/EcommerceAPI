using Ecommerce.Modules.Mails.Api.Services;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Events.Externals.Handlers
{
    internal class CustomerPlacedOrderHandler : IEventHandler<CustomerPlacedOrder>
    {
        private readonly IMailService _mailService;

        public CustomerPlacedOrderHandler(IMailService mailService)
        {
            _mailService = mailService;
        }
        public Task HandleAsync(CustomerPlacedOrder @event)
        {
            throw new NotImplementedException();
        }
    }
}
