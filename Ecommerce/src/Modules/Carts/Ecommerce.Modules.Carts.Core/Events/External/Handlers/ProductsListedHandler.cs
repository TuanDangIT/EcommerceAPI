using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Events.External.Handlers
{
    internal class ProductsListedHandler : IEventHandler<ProductsListed>
    {
        private readonly ICartsDbContext _dbContext;

        public ProductsListedHandler(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(ProductsListed @event)
        {
            var products = new List<Product>();
            foreach(var productObject in @event.Products)
            {
                var product = JsonSerializer.Deserialize<Product>(JsonSerializer.Serialize(productObject));
                if (product is null)
                {
                    throw new ArgumentNullException(nameof(@event));
                }
                products.Add(product);
            }
            await _dbContext.Products.AddRangeAsync(products);
            await _dbContext.SaveChangesAsync();
        }
    }
}
