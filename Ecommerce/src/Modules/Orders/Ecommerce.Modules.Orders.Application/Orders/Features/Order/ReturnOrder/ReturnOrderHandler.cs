﻿using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Policies;
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
        private readonly IOrderReturnPolicy _orderReturnPolicy;
        private readonly TimeProvider _timeProvider;
        private readonly IMessageBroker _messageBroker;

        public ReturnOrderHandler(IOrderRepository orderRepository, IDomainEventDispatcher domainEventDispatcher, IOrderReturnPolicy orderReturnPolicy, TimeProvider timeProvider,
            IMessageBroker messageBroker)
        {
            _orderRepository = orderRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _orderReturnPolicy = orderReturnPolicy;
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
            var returnProducts = order.Products
                .Where(p => request.ProductsToReturn.Select(ptr => ptr.SKU).Contains(p.SKU))
                .Select(p =>
                {
                    var returnedQuantity = request.ProductsToReturn.Single(ptr => ptr.SKU == p.SKU).Quantity;
                    p = new Domain.Orders.Entities.Product(p.SKU, p.Name, p.Price, returnedQuantity, p.ImagePathUrl);
                    return p;
                }).ToList();
            foreach(var product in request.ProductsToReturn)
            {
                order.DecreaseProductQuantity(product.SKU, product.Quantity);
            }
            //usuwanie w order tutaj dać logikę
            if(!await _orderReturnPolicy.CanReturn(order))
            {
                throw new OrderCannotReturnException("Cannot return an order after 14 days of placing it.");
            }
            bool isFullReturn = !order.Products.Any();
            await _domainEventDispatcher.DispatchAsync(new OrderReturned(request.ReasonForReturn, order.Id, returnProducts, isFullReturn, now));
            order.Return(now);
            await _orderRepository.UpdateAsync();
            //More logic here
        }
    }
}
