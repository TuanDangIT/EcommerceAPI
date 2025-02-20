using Coravel.Invocable;
using Coravel.Scheduling.Schedule.Interfaces;
using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Events;
using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Shared.Infrastructure.Carts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Scheduler
{
    internal class CleanupAbandonedCartsTask : IInvocable
    {
        private readonly ICartsDbContext _dbContext;
        private readonly IMessageBroker _messageBroker;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<CleanupAbandonedCartsTask> _logger;
        private readonly CartOptions _cartOptions;

        public CleanupAbandonedCartsTask(ICartsDbContext dbContext, IMessageBroker messageBroker,
            TimeProvider timeProvider, ILogger<CleanupAbandonedCartsTask> logger, CartOptions cartOptions)
        {
            _dbContext = dbContext;
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
            _logger = logger;
            _cartOptions = cartOptions;
        }
        public async Task Invoke()
        {
            var now = _timeProvider.GetUtcNow();
            var carts = await _dbContext.Carts
                .Include(c => c.Products)
                .ThenInclude(cp => cp.Product)
                .Where(c => (c.UpdatedAt != null && (c.UpdatedAt + TimeSpan.FromDays(_cartOptions.LifeTime)).Value <= now) ||
                    (c.UpdatedAt == null && (c.CreatedAt + TimeSpan.FromDays(_cartOptions.LifeTime * 2)) <= now))
                .ToListAsync();
            if (carts.Count == 0)
            {
                _logger.LogDebug("No carts were cleaned up.");
                return;
            }
            var products = carts.SelectMany(c => c.Products)
                .GroupBy(cp => cp.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(cp => cp.Quantity));
            foreach (var cart in carts)
            {
                cart.Clear();
            }
            if (products.Count > 0)
            {
                await _messageBroker.PublishAsync(new ProductsUnreserved(products));
            }
            _dbContext.Carts.RemoveRange(carts);
            await _dbContext.SaveChangesAsync();
            _logger.LogDebug("Cleaned up abandoned carts.");
        }
    }
}
