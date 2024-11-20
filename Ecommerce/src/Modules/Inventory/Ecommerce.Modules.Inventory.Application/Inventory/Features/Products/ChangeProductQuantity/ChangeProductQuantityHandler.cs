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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ChangeProductQuantity
{
    internal class ChangeProductQuantityHandler : ICommandHandler<ChangeProductQuantity>
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryEventMapper _inventoryEventMapper;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IMessageBroker _messageBroker;

        public ChangeProductQuantityHandler(IProductRepository productRepository, IInventoryEventMapper inventoryEventMapper, 
            IDomainEventDispatcher domainEventDispatcher, IMessageBroker messageBroker)
        {
            _productRepository = productRepository;
            _inventoryEventMapper = inventoryEventMapper;
            _domainEventDispatcher = domainEventDispatcher;
            _messageBroker = messageBroker;
        }
        public async Task Handle(ChangeProductQuantity request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId);
            if(product is null)
            {
                throw new ProductNotFoundException(request.ProductId);
            }
            product.ChangeQuantity(request.Quantity);
            await _productRepository.UpdateAsync();
            var domainEvent = new ProductQuantityChanged(product.Id, request.Quantity);
            await _domainEventDispatcher.DispatchAsync(domainEvent);
            var integrationEvent = _inventoryEventMapper.Map(domainEvent);
            await _messageBroker.PublishAsync(integrationEvent);
        }
    }
}
