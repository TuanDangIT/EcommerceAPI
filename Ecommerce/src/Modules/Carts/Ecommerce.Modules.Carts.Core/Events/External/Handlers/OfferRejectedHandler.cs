using Ecommerce.Modules.Carts.Core.DAL;
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
    internal class OfferRejectedHandler : IEventHandler<OfferRejected>
    {
        private readonly ICartsDbContext _dbContext;

        public OfferRejectedHandler(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(OfferRejected @event)
        {
            if(@event.Code is null)
            {
                return;
            }
            await _dbContext.Discounts
                .Where(d => d.Code == @event.Code)
                .ExecuteDeleteAsync();
        }
    }
}
