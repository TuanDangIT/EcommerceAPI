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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ListProducts
{
    internal sealed class ListProductsHandler : ICommandHandler<ListProducts>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly TimeProvider _timeProvider;
        private readonly IInventoryEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<ListProductsHandler> _logger;
        private readonly IContextService _contextService;

        public ListProductsHandler(IProductRepository productRepository, IDomainEventDispatcher domainEventDispatcher,
            TimeProvider timeProvider, IInventoryEventMapper eventMapper, IMessageBroker messageBroker, ILogger<ListProductsHandler> logger,
            IContextService contextService)
        {
            _productRepository = productRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _timeProvider = timeProvider;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(ListProducts request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllThatContainsInArrayAsync(request.ProductIds, cancellationToken);
            if(products.Count() != request.ProductIds.Length)
            {
                var productIdsNotFound = new List<Guid>();
                foreach(var productId in request.ProductIds)
                {
                    if(products.Select(p => p.Id).Contains(productId))
                    {
                        productIdsNotFound.Add(productId);
                    }
                }
                throw new ProductNotAllFoundException(productIdsNotFound);
            }
            await _productRepository.UpdateListedFlagAsync(request.ProductIds, true, cancellationToken);
            _logger.LogInformation("Products: {@productIds} were listed by {@user}.",
                products.Select(p => p.Id), new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            var domainEvent = new ProductsListed(products, _timeProvider.GetUtcNow().UtcDateTime);
            await _domainEventDispatcher.DispatchAsync(domainEvent);
            var integrationEvent = _eventMapper.Map(domainEvent);
            await _messageBroker.PublishAsync(integrationEvent);
        }
    }
}
