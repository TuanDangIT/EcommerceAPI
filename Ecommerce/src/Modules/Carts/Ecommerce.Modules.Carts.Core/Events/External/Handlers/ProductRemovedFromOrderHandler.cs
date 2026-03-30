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
    internal class ProductRemovedFromOrderHandler : IEventHandler<ProductRemovedFromOrder>
    {
        private readonly ICartsDbContext _dbContext;

        public ProductRemovedFromOrderHandler(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(ProductRemovedFromOrder @event)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == @event.ProductId);    

            if (product is not null && product.HasQuantity)
            {
                product.IncreaseQuantity(@event.Quantity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
