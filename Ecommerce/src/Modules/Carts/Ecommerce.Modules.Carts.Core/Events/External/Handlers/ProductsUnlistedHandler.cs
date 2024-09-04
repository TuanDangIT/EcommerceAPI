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
    internal class ProductsUnlistedHandler : IEventHandler<ProductsUnlisted>
    {
        private readonly ICartsDbContext _dbContext;

        public ProductsUnlistedHandler(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(ProductsUnlisted @event)
        {
            var rowsChanged = await _dbContext.Products.Where(p => @event.ProductIds.Contains(p.Id)).ExecuteDeleteAsync();
            if(rowsChanged != @event.ProductIds.Count())
            {
                throw new ProductsNotDeletedException();
            }
        }
    }
}
