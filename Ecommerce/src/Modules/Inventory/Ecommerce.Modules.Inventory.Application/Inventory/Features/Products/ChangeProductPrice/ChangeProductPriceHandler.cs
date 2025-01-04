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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ChangeProductPrice
{
    internal class ChangeProductPriceHandler : ICommandHandler<ChangeProductPrice>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ChangeProductPriceHandler> _logger;
        private readonly IContextService _contextService;

        public ChangeProductPriceHandler(IProductRepository productRepository, ILogger<ChangeProductPriceHandler> logger,
            IContextService contextService)
        {
            _productRepository = productRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(ChangeProductPrice request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId, cancellationToken) ?? 
                throw new ProductNotFoundException(request.ProductId);
            product.ChangePrice(request.Price);
            await _productRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Product's: {productId} price was changed from {oldPrice} to {newPrice} by {@user}.",
                product.Id, product.Price, request.Price, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
