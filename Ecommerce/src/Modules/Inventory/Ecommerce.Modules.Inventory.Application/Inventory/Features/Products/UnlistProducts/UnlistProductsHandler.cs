using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Application.Inventory.Services;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.UnlistProducts
{
    public sealed class UnlistProductsHandler : ICommandHandler<UnlistProducts>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IInventoryEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<UnlistProductsHandler> _logger;
        private readonly IContextService _contextService;

        public UnlistProductsHandler(IProductRepository productRepository, IDomainEventDispatcher domainEventDispatcher,
            IInventoryEventMapper eventMapper, IMessageBroker messageBroker, ILogger<UnlistProductsHandler> logger,
            IContextService contextService)
        {
            _productRepository = productRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(UnlistProducts request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllThatContainsInArrayAsync(request.ProductIds, cancellationToken);
            var productIds = products.Select(p => p.Id);
            if (products.Count() != request.ProductIds.Length)
            {
                var productIdsNotFound = new List<Guid>();
                foreach (var productId in request.ProductIds)
                {
                    if (!productIds.Contains(productId))
                    {
                        productIdsNotFound.Add(productId);
                    }
                }
                throw new ProductNotAllFoundException(productIdsNotFound);
            }
            var productsToUnlist = products.ToList();
            foreach (var product in products)
            {
                if(product.IsListed == false)
                {
                    _logger.LogWarning("Product: {productId} was not unlisted, because it isListed flag is false.", product.Id);
                    productsToUnlist.Remove(product);
                }
            }
            await _productRepository.UpdateListedFlagAsync(request.ProductIds, false, cancellationToken);
            _logger.LogInformation("Products: {@productsIds} were listed by {@user}.",
                string.Join(", ", products.Select(p => p.Id)), new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            var domainEvent = new ProductsUnlisted(productIds);
            await _domainEventDispatcher.DispatchAsync(domainEvent);
            var integrationEvent = _eventMapper.Map(domainEvent);
            await _messageBroker.PublishAsync(integrationEvent);
        }
    }
}
