using Ecommerce.Modules.Orders.Application.Orders.Events;
using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Application.Stripe;
using Ecommerce.Modules.Orders.Domain.Orders.Policies;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
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
        private readonly IStripeService _stripeService;
        private readonly IMessageBroker _messageBroker;
        private readonly IOrderCancellationPolicy _orderCancellationPolicy;

        public CancelOrderHandler(IOrderRepository orderRepository, IStripeService stripeService, IMessageBroker messageBroker, IOrderCancellationPolicy orderCancellationPolicy)
        {
            _orderRepository = orderRepository;
            _stripeService = stripeService;
            _messageBroker = messageBroker;
            _orderCancellationPolicy = orderCancellationPolicy;
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
            await _stripeService.Refund(order);
            order.Cancel();
            await _orderRepository.UpdateAsync();
            var products = order.Products;
            await _messageBroker.PublishAsync(new OrderCancelled(order.Id, order.Customer.UserId, order.Customer.FirstName, order.Customer.Email, products.Select(p => new { p.SKU, p.Quantity }), order.CreatedAt));
        }
    }
}
