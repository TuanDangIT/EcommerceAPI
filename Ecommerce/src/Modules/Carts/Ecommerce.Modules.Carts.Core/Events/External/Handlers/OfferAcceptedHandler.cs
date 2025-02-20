using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Events.External.Handlers
{
    internal class OfferAcceptedHandler : IEventHandler<OfferAccepted>
    {
        private readonly ICartsDbContext _dbContext;
        private readonly TimeProvider _timeProvider;

        public OfferAcceptedHandler(ICartsDbContext dbContext, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _timeProvider = timeProvider;
        }
        public async Task HandleAsync(OfferAccepted @event)
        {
            var discount = new Discount(@event.Code, @event.SKU, @event.OldPrice - @event.OfferedPrice, @event.CustomerId, @event.ExpiresAt,
                _timeProvider.GetUtcNow().DateTime);
            await _dbContext.Discounts.AddAsync(discount);
            await _dbContext.SaveChangesAsync();
        }
    }
}
