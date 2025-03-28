using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Policies;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
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
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken,
                query => query.Include(o => o.Products),
                query => query.Include(o => o.Customer)) ?? 
                throw new OrderNotFoundException(request.OrderId);

            var orderStatus = order.Status;
            if (orderStatus == OrderStatus.Draft)
            {
                throw new OrderDraftException(order.Id);
            }
            //if (orderStatus == OrderStatus.ReturnRejected)
            //{
            //    throw new OrderCannotReturnRejectedReturnException(order.Id);
            //}
            if (orderStatus == OrderStatus.Cancelled)
            {
                throw new OrderInvalidStatusChangeException($"Order: {order.Id} cannot be cancelled.");
            }
            if(!await _orderReturnPolicy.CanReturn(order))
            {
                throw new OrderCannotReturnException("Cannot return an order after 14 days of placing it.");
            }
            var returnProducts = order.Products
                .Where(p => request.ProductsToReturn.Select(ptr => ptr.ProductId).Contains(p.Id))
                .Select(p =>
                {
                    var returnQuantity = request.ProductsToReturn.Single(ptr => ptr.ProductId == p.Id).Quantity;
                    p = new Domain.Orders.Entities.Product(p.SKU, p.Name, p.UnitPrice, returnQuantity, p.ImagePathUrl);
                    return p;
                }).ToList();
            order.ReturnProducts(request.ProductsToReturn.Select(p => (p.ProductId, p.Quantity)));
            await _orderRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Order: {orderId} was returned. Products that were returned: {@productToReturn}.", order.Id, request.ProductsToReturn);
            if(orderStatus == OrderStatus.Returned)
            {
                await _domainEventDispatcher.DispatchAsync(new OrderReturnCorrected(order.Id, returnProducts));
                return;
            }
            bool isFullReturn = !order.Products.Any();
            var domainEvent = new OrderReturned(order.Id, order.Customer!.UserId, order.Customer.FirstName, order.Customer.Email, request.ReasonForReturn, returnProducts, isFullReturn);
            await _domainEventDispatcher.DispatchAsync(domainEvent);
            var integrationEvent = _ordersEventMapper.Map(domainEvent);
            await _messageBroker.PublishAsync(integrationEvent);
        }
    }
}
