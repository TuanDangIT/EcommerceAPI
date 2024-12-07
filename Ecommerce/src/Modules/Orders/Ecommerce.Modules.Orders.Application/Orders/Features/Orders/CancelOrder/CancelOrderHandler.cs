﻿using Ecommerce.Modules.Orders.Application.Orders.Events;
using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Application.Shared.Stripe;
using Ecommerce.Modules.Orders.Domain.Orders.Policies;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.CancelOrder
{
    internal class CancelOrderHandler : ICommandHandler<CancelOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentProcessorService _stripeService;
        private readonly IMessageBroker _messageBroker;
        private readonly IOrderCancellationPolicy _orderCancellationPolicy;
        private readonly ILogger<CancelOrderHandler> _logger;
        private readonly IContextService _contextService;

        public CancelOrderHandler(IOrderRepository orderRepository, IPaymentProcessorService stripeService, IMessageBroker messageBroker, IOrderCancellationPolicy orderCancellationPolicy,
            ILogger<CancelOrderHandler> logger, IContextService contextService)
        {
            _orderRepository = orderRepository;
            _stripeService = stripeService;
            _messageBroker = messageBroker;
            _orderCancellationPolicy = orderCancellationPolicy;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(CancelOrder request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            if (!await _orderCancellationPolicy.CanCancel(order))
            {
                throw new OrderCannotCancelException();
            }
            await _stripeService.RefundAsync(order);
            order.Cancel();
            await _orderRepository.UpdateAsync();
            var products = order.Products;
            _logger.LogInformation("Order: {order} was cancelled.", order);
            await _messageBroker.PublishAsync(new OrderCancelled(order.Id, order.Customer.UserId, order.Customer.FirstName, order.Customer.Email, products.Select(p => new { p.SKU, p.Quantity }), order.CreatedAt));
        }
    }
}
