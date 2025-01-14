using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Events.Externals.Handlers
{
    internal class ProductsUnreservedHandler : IEventHandler<ProductsUnreserved>
    {
        private readonly IProductRepository _productRepository;

        public ProductsUnreservedHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task HandleAsync(ProductsUnreserved @event)
        {
            await using var transaction = await _productRepository.BeginTransactionAsync();
            try
            {
                var products = await _productRepository.GetAllThatContainsInArrayAsync(@event.Products.Select(p => p.Key).ToArray());
                foreach (var productKeyValuePair in @event.Products)
                {
                    var product = products.SingleOrDefault(p => p.Id == productKeyValuePair.Key);
                    if(product is null || !product.HasQuantity)
                    {
                        continue;
                    }
                    product.Unreserve(productKeyValuePair.Value);
                }
                await _productRepository.UpdateAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
