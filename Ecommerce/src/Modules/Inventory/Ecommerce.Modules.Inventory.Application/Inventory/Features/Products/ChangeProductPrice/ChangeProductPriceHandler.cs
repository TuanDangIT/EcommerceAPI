using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Application.Inventory.Services;
using Ecommerce.Modules.Inventory.Domain.Inventory.Events;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
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
        private readonly IInventoryEventMapper _inventoryEventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public ChangeProductPriceHandler(IProductRepository productRepository, ILogger<ChangeProductPriceHandler> logger,
            IContextService contextService, IInventoryEventMapper inventoryEventMapper, IMessageBroker messageBroker,
            IDomainEventDispatcher domainEventDispatcher)
        {
            _productRepository = productRepository;
            _logger = logger;
            _contextService = contextService;
            _inventoryEventMapper = inventoryEventMapper;
            _messageBroker = messageBroker;
            _domainEventDispatcher = domainEventDispatcher;
        }
        public async Task Handle(ChangeProductPrice request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId, cancellationToken) ?? 
                throw new ProductNotFoundException(request.ProductId);
            product.ChangePrice(request.Price);
            await _productRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Product's: {productId} price was changed from {oldPrice} to {newPrice} by {@user}.",
                product.Id, product.Price, request.Price, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            if (product.IsListed)
            {
                var domainEvent = new ProductPriceChanged(product.Id, request.Price);
                await _domainEventDispatcher.DispatchAsync(domainEvent);
                var integrationEvent = _inventoryEventMapper.Map(domainEvent);
                await _messageBroker.PublishAsync(integrationEvent);
            }
        }
    }
}
