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
    internal class ProductQuantityChangedHandler : IEventHandler<ProductQuantityChanged>
    {
        private readonly ICartsDbContext _dbContext;

        public ProductQuantityChangedHandler(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(ProductQuantityChanged @event)
        {
            var product = await _dbContext.Products
                .SingleOrDefaultAsync(p => p.Id == @event.ProductId);
            if(product is null)
            {
                throw new ProductNotFoundException(@event.ProductId);
            }
            product.DecreaseQuantity(@event.Quantity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
