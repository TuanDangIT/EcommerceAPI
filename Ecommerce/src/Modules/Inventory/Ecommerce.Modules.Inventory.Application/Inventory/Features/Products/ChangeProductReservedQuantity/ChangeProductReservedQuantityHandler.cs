using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ChangeProductReservedQuantity
{
    internal class ChangeProductReservedQuantityHandler : ICommandHandler<ChangeProductReservedQuantity>
    {
        private readonly IProductRepository _productRepository;

        public ChangeProductReservedQuantityHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task Handle(ChangeProductReservedQuantity request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId);
            if (product is null)
            {
                throw new ProductNotFoundException(request.ProductId);
            }
            product.ChangeProductReservedQuantity(request.Reserved);
            await _productRepository.UpdateAsync();
        }
    }
}
