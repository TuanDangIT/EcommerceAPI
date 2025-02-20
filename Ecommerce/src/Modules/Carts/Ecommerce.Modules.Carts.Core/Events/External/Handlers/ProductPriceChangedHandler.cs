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
    internal class ProductPriceChangedHandler : IEventHandler<ProductPriceChanged>
    {
        private readonly ICartsDbContext _dbContext;

        public ProductPriceChangedHandler(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(ProductPriceChanged @event)
        {
            var product = await _dbContext.Products
                .SingleOrDefaultAsync(p => p.Id == @event.ProductId) ?? 
                throw new ProductNotFoundException(@event.ProductId);
            product.ChangePrice(@event.Price);
            await _dbContext.SaveChangesAsync();
        }
    }
}
