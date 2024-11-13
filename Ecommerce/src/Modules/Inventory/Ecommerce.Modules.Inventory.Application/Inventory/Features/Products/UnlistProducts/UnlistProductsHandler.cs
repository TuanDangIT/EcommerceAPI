using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Application.Inventory.Services;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Events;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
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

        public UnlistProductsHandler(IProductRepository productRepository, IDomainEventDispatcher domainEventDispatcher,
            IInventoryEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _productRepository = productRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        public async Task Handle(UnlistProducts request, CancellationToken cancellationToken)
        {
            var productIds = await _productRepository.GetAllIdThatContainsInArrayAsync(request.ProductIds);
            if (productIds.Count() != request.ProductIds.Length)
            {
                var productIdsNotFound = new List<Guid>();
                foreach (var productId in request.ProductIds)
                {
                    if (productIds.Contains(productId))
                    {
                        productIdsNotFound.Add(productId);
                    }
                }
                throw new ProductNotAllFoundException(productIdsNotFound);
            }
            await _productRepository.UpdateListedFlagAsync(request.ProductIds, false);
            var domainEvent = new ProductsUnlisted(productIds);
            await _domainEventDispatcher.DispatchAsync(domainEvent);
            var integrationEvent = _eventMapper.Map(domainEvent);
            await _messageBroker.PublishAsync(integrationEvent);
        }
    }
}
