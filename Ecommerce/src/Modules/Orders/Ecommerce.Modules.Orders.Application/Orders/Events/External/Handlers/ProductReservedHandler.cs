using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Events.External.Handlers
{
    internal class ProductReservedHandler : IEventHandler<ProductReserved>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductReservedHandler> _logger;

        public ProductReservedHandler(IProductRepository productRepository, ILogger<ProductReservedHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        public async Task HandleAsync(ProductReserved @event)
        {
            var product = await _productRepository.GetAsync(@event.ProductId);

            if (product is null) _logger.LogWarning("Product with id: {ProductId} not found", @event.ProductId);

            product!.DecreaseQuantity(@event.Quantity);
        }
    }
}
