using Ecommerce.Modules.Mails.Api.DAL;
using Ecommerce.Modules.Mails.Api.Entities;
using Ecommerce.Modules.Mails.Api.Events.Externals;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Events.Externals.Handlers
{
    internal class CustomerActivatedHandler : IEventHandler<CustomerActivated>
    {
        private readonly IMailsDbContext _dbContext;

        public CustomerActivatedHandler(IMailsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(CustomerActivated @event)
        {
            await _dbContext.Customers.AddAsync(new Customer(@event.CustomerId, @event.Email, @event.FirstName, @event.LastName));
            await _dbContext.SaveChangesAsync();
        }
    }
}
