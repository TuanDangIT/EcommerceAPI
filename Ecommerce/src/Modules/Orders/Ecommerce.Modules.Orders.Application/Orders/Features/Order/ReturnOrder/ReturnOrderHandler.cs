using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.ReturnOrder
{
    internal sealed class ReturnOrderHandler : ICommandHandler<ReturnOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly TimeProvider _timeProvider;
        private readonly IMessageBroker _messageBroker;

        public ReturnOrderHandler(IOrderRepository orderRepository, IDomainEventDispatcher domainEventDispatcher, TimeProvider timeProvider,
            IMessageBroker messageBroker)
        {
            _orderRepository = orderRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _timeProvider = timeProvider;
            _messageBroker = messageBroker;
        }
        public async Task Handle(ReturnOrder request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderAsync(request.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            var now = _timeProvider.GetUtcNow().UtcDateTime;
            foreach(var product in request.ProductsToReturn)
            {
                order.DecreaseProductQuantity(product.SKU, product.Quantity);
            }
            //usuwanie w order tutaj dać logikę
            order.Return(now);
            await _orderRepository.UpdateAsync();
            var orderProducts = order.Products
                .Where(p => request.ProductsToReturn.Select(ptr => ptr.SKU).Contains(p.SKU))
                .Select(p =>
                {
                    var returnedQuantity = request.ProductsToReturn.Single(ptr => ptr.SKU == p.SKU).Quantity;
                    p = new Domain.Orders.Entities.Product(p.SKU, p.Name, p.Price, returnedQuantity, p.ImagePathUrl);
                    return p;
                });
            bool isFullReturn = orderProducts.Count() == order.Products.Count();
            await _domainEventDispatcher.DispatchAsync(new OrderReturned(request.ReasonForReturn, order.Id, orderProducts, isFullReturn, now));
            //More logic here
        }
    }
}
