using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.DecreaseProductsQuantity
{
    internal class DecreaseProductsQuantityHandler : ICommandHandler<DecreaseProductsQuantity>
    {
        private readonly IProductRepository _productRepository;

        public DecreaseProductsQuantityHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task Handle(DecreaseProductsQuantity request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId);
            if(product is null)
            {
                throw new ProductNotFoundException(request.ProductId);
            }
            product.DecreaseQuantity(request.Quantity);
            var rowsChanged = await _productRepository.UpdateAsync();
            if(rowsChanged is not 0)
            {
                throw new ProductNotDecreasedException(request.ProductId);
            }
        }
    }
}
