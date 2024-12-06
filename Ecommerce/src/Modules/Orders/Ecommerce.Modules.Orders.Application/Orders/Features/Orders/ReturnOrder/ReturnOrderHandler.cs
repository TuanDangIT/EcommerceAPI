using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Policies;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
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
        private readonly IOrdersEventMapper _ordersEventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<ReturnOrderHandler> _logger;

        public ReturnOrderHandler(IOrderRepository orderRepository, IDomainEventDispatcher domainEventDispatcher, IOrderReturnPolicy orderReturnPolicy, 
            IOrdersEventMapper ordersEventMapper, IMessageBroker messageBroker, ILogger<ReturnOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _orderReturnPolicy = orderReturnPolicy;
            _ordersEventMapper = ordersEventMapper;
            _messageBroker = messageBroker;
            _logger = logger;
        }
        public async Task Handle(ReturnOrder request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
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
            var domainEvent = new OrderReturned(order.Id, order.Customer.UserId, order.Customer.FirstName, order.Customer.Email, request.ReasonForReturn, returnProducts, isFullReturn);
            await _domainEventDispatcher.DispatchAsync(domainEvent);
            var integrationEvent = _ordersEventMapper.Map(domainEvent);
            await _messageBroker.PublishAsync(integrationEvent);
            order.Return();
            await _orderRepository.UpdateAsync();
            _logger.LogInformation("Order: {order} was returned.", order);
        }
    }
}
