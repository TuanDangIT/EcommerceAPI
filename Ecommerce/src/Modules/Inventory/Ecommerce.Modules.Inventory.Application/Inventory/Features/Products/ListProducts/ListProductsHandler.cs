using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Application.Inventory.Services;
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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ListProducts
{
    internal sealed class ListProductsHandler : ICommandHandler<ListProducts>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly TimeProvider _timeProvider;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public ListProductsHandler(IProductRepository productRepository, IDomainEventDispatcher domainEventDispatcher,
            TimeProvider timeProvider, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _productRepository = productRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _timeProvider = timeProvider;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        public async Task Handle(ListProducts request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllThatContainsInArrayAsync(request.ProductIds);
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
            var domainEvent = new ProductListed(products, _timeProvider.GetUtcNow().UtcDateTime);
            await _domainEventDispatcher.DispatchAsync(domainEvent);
            var integrationEvent = _eventMapper.Map(domainEvent);
            await _messageBroker.PublishAsync(integrationEvent);
        }
    }
}
