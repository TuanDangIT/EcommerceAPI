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
    internal class ReturnHandledHandler : IEventHandler<ReturnHandled>
    {
        private readonly ICartsDbContext _dbContext;

        public ReturnHandledHandler(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task HandleAsync(ReturnHandled @event)
        {
            var products = await _dbContext.Products
                .Where(p => @event.Products.Select(ep => ep.SKU).ToArray().Contains(p.SKU))
                .ToListAsync();
            foreach(var product in products)
            {
                var quantityToIncrease = @event.Products.Single(p => p.SKU == product.SKU).Quantity;
                if (product.HasQuantity)
                {
                    product.IncreaseQuantity((int)quantityToIncrease!);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
