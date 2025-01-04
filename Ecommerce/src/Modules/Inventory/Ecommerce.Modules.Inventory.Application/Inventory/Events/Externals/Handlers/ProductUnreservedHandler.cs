using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Events.Externals.Handlers
{
    internal class ProductUnreservedHandler : IEventHandler<ProductUnreserved>
    {
        private readonly IProductRepository _productRepository;

        public ProductUnreservedHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task HandleAsync(ProductUnreserved @event)
        {
            await using var transaction = await _productRepository.BeginTransactionAsync();
            try
            {
                var product = await _productRepository.GetAsync(@event.ProductId) ?? throw new ProductNotFoundException(@event.ProductId);
                product.Unreserve(@event.Quantity);
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
