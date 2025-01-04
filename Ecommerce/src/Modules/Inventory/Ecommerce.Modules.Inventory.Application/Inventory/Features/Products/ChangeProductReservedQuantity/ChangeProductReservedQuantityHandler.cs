using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ChangeProductReservedQuantityHandler> _logger;
        private readonly IContextService _contextService;

        public ChangeProductReservedQuantityHandler(IProductRepository productRepository, ILogger<ChangeProductReservedQuantityHandler> logger,
            IContextService contextService)
        {
            _productRepository = productRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(ChangeProductReservedQuantity request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId, cancellationToken) ?? 
                throw new ProductNotFoundException(request.ProductId);
            product.ChangeReservedQuantity(request.Reserved);
            await _productRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Product's: {productId} reserved quantity was changed from {oldReservedQuantity} to {newReservedQuantity} by {@user}.",
                product.Id, product.Reserved, request.Reserved, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
