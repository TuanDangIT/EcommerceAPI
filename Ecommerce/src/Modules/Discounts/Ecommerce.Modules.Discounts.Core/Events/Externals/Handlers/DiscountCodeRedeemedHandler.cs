using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Events.Externals.Handlers
{
    internal class DiscountCodeRedeemedHandler : IEventHandler<DiscountCodeRedeemed>
    {
        private readonly IDiscountDbContext _dbContext;
        private readonly TimeProvider _timeProvider;

        public DiscountCodeRedeemedHandler(IDiscountDbContext dbContext, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _timeProvider = timeProvider;
        }
        public async Task HandleAsync(DiscountCodeRedeemed @event)
        {
            var discount = await _dbContext.Discounts
                .SingleOrDefaultAsync(d => d.Code == @event.Code);
            if(discount is null)
            {
                throw new DiscountNotFoundException(@event.Code);
            }
            discount.Redeem();
            await _dbContext.SaveChangesAsync();
        }
    }
}
