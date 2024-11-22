using Ecommerce.Modules.Carts.Core.DAL;
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
    internal class ProductQuantityChangedHandler : IEventHandler<ProductQuantityChanged>
    {
        private readonly ICartsDbContext _dbContext;
        private readonly ILogger<ProductQuantityChangedHandler> _logger;

        public ProductQuantityChangedHandler(ICartsDbContext dbContext, ILogger<ProductQuantityChangedHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task HandleAsync(ProductQuantityChanged @event)
        {
            var product = await _dbContext.Products
                .SingleOrDefaultAsync(p => p.Id == @event.ProductId);
            if(product is null)
            {
                _logger.LogError("Product: {productId} was not found.", @event.ProductId);
                throw new ProductNotFoundException(@event.ProductId);
            }
            product.DecreaseQuantity(@event.Quantity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
