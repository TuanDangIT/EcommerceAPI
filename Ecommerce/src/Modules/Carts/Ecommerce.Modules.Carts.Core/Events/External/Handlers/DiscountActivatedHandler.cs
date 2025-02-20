using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Events.External.Handlers
{
    internal class DiscountActivatedHandler : IEventHandler<DiscountActivated>
    {
        private readonly ICartsDbContext _dbContext;
        private readonly ILogger<DiscountActivatedHandler> _logger;
        private readonly TimeProvider _timeProvider;
        private const string _nominalCouponType = "NominalCoupon";
        private const string _percentageCouponType = "PercentageCoupon";
      

        public DiscountActivatedHandler(ICartsDbContext dbContext, ILogger<DiscountActivatedHandler> logger,
            TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _logger = logger;
            _timeProvider = timeProvider;
        }
        public async Task HandleAsync(DiscountActivated @event)
        {
            var discount = await _dbContext.Discounts
                .SingleOrDefaultAsync(d => d.Code == @event.Code);
            if(discount is not null)
            {
                throw new DiscountCodeAlreadyInUseException(@event.Code);
            }
            switch (@event.Type)
            {
                case _nominalCouponType:
                    await _dbContext.Discounts
                        .AddAsync(new Entities.Discount(@event.Code, DiscountType.NominalDiscount, @event.Value, @event.ExpiresAt, 
                        @event.StripePromotionCodeId, _timeProvider.GetUtcNow().DateTime));
                    break;
                case _percentageCouponType:
                    await _dbContext.Discounts
                        .AddAsync(new Entities.Discount(@event.Code, DiscountType.PercentageDiscount, @event.Value, @event.ExpiresAt,
                        @event.StripePromotionCodeId, _timeProvider.GetUtcNow().DateTime));
                    break;
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
