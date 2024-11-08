using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Events.Externals.Handlers
{
    internal class OfferRequestedHandler : IEventHandler<OfferRequested>
    {
        private readonly IDiscountDbContext _dbContext;

        public OfferRequestedHandler(IDiscountDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(OfferRequested @event)
        {
            var offer = new Offer(@event.SKU, @event.ProductName, @event.OfferedPrice, @event.OldPrice, @event.Reason, @event.CustomerId);
            await _dbContext.Offers.AddAsync(offer);
            await _dbContext.SaveChangesAsync();
        }
    }
}
