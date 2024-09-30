using Ecommerce.Modules.Carts.Core.DAL;
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
    internal class DiscountDeactivatedHandler : IEventHandler<DiscountDeactivated>
    {
        private readonly ICartsDbContext _dbContext;

        public DiscountDeactivatedHandler(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(DiscountDeactivated @event)
            => await _dbContext.Discounts
                .Where(d => d.Code == @event.Code)
                .ExecuteDeleteAsync();
    }
}
