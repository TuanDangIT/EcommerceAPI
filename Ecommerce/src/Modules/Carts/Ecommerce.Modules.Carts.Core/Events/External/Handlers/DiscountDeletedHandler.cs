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
    internal class DiscountDeletedHandler : IEventHandler<DiscountDeleted>
    {
        private readonly ICartsDbContext _dbContext;

        public DiscountDeletedHandler(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(DiscountDeleted @event)
            => await _dbContext.Discounts
                .Where(d => d.Code == @event.Code)
                .ExecuteDeleteAsync();
    }
}
