using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Events.External.Handlers
{
    internal class DiscountCreatedHandler : IEventHandler<DiscountCreated>
    {
        private readonly ICartsDbContext _dbContext;

        public DiscountCreatedHandler(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(DiscountCreated @event)
        {
            var discount = await _dbContext.Discounts
                .SingleOrDefaultAsync(d => d.Code == @event.Code);
            if(discount is not null)
            {
                throw new DiscountCodeAlreadyInUseException(@event.Code);
            }
            if(!Enum.TryParse(typeof(DiscountType), @event.Type, out var discountType))
            {
                throw new DiscountInvalidTypeException();
            }
            await _dbContext.Discounts
                .AddAsync(new Entities.Discount(@event.Code, (DiscountType)discountType!, @event.Value, @event.EndingDate));
            await _dbContext.SaveChangesAsync();
        }
    }
}
