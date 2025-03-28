using Ecommerce.Modules.Orders.Application.Orders.Events;
using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.HandleOrderShipped
{
    internal class HandlerOrderShippedHandler : ICommandHandler<HandlerOrderShipped>
    {
        private const string _inPostPayloadPropertyName = "payload";
        private const string _inPostTrackingNumberPropertyName = "tracking_number";
        private const string _inPostStatusPropertyName = "status";
        private const string _inPostDeliveredStatus = "collected_from_sender";
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<HandlerOrderShippedHandler> _logger;

        public HandlerOrderShippedHandler(IOrderRepository orderRepository, IMessageBroker messageBroker,
            ILogger<HandlerOrderShippedHandler> logger)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _logger = logger;
        }
        public async Task Handle(HandlerOrderShipped request, CancellationToken cancellationToken)
        {
            using var jsonDocument = JsonDocument.Parse(request.Json);
            var payload = jsonDocument.RootElement.GetProperty(_inPostPayloadPropertyName);
            var shipmentStatus = payload.GetProperty(_inPostStatusPropertyName).GetString();
            if (shipmentStatus is null || shipmentStatus != _inPostDeliveredStatus)
            {
                return;
            }
            var trackingNumber = payload.GetProperty(_inPostTrackingNumberPropertyName).GetString();
            if (trackingNumber is null)
            {
                return;
            }
            var order = await _orderRepository.GetAsync(trackingNumber, cancellationToken) ?? throw new OrderNotFoundException(trackingNumber);
            order.Ship();
            await _orderRepository.UpdateAsync(cancellationToken);
            await _messageBroker.PublishAsync(new OrderShipped(order.Id, order.Customer!.UserId, order.Customer.FirstName, order.Customer.Email, order.CreatedAt));
            _logger.LogInformation("Order's: {@order} was changed to shipped by InPost webhook.", order);
        }
    }
}
