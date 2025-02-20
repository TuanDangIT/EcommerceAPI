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

        public DiscountCodeRedeemedHandler(IDiscountDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(DiscountCodeRedeemed @event)
        {
            var discount = await _dbContext.Discounts
                .SingleOrDefaultAsync(d => d.Code == @event.Code) ?? 
                throw new DiscountNotFoundException(@event.Code);
            discount.Redeem();
            await _dbContext.SaveChangesAsync();
        }
    }
}
