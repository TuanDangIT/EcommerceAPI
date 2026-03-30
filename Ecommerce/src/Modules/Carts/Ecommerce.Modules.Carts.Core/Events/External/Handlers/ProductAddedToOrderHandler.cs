using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Shared.Abstractions.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Events.External.Handlers
{
    internal class ProductAddedToOrderHandler : IEventHandler<ProductAddedToOrder>
    {
        private readonly ICartsDbContext _dbContext;

        public ProductAddedToOrderHandler(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(ProductAddedToOrder @event)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == @event.ProductId);

            if (product is not null && product.HasQuantity)
            {
                product.DecreaseQuantity(@event.Quantity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
