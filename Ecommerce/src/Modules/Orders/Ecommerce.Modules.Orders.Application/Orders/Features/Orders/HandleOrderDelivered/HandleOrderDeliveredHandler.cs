using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.HandleOrderDelivered
{
    internal class HandleOrderDeliveredHandler : ICommandHandler<HandleOrderDelivered>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<HandleOrderDeliveredHandler> _logger;
        private const string _inPostPayloadPropertyName = "payload";
        private const string _inPostTrackingNumberPropertyName = "tracking_number";
        private const string _inPostStatusPropertyName = "status";
        private const string _inPostDeliveredStatus = "delivered";

        public HandleOrderDeliveredHandler(IOrderRepository orderRepository, ILogger<HandleOrderDeliveredHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task Handle(HandleOrderDelivered request, CancellationToken cancellationToken)
        {
            using var jsonDocument = JsonDocument.Parse(request.Json);
            var payload = jsonDocument.RootElement.GetProperty(_inPostPayloadPropertyName);
            var shipmentStatus = payload.GetProperty(_inPostStatusPropertyName).GetString();
            if(shipmentStatus is null || shipmentStatus != _inPostDeliveredStatus)
            {
                return;
            }
            var trackingNumber = payload.GetProperty(_inPostTrackingNumberPropertyName).GetString();
            if (trackingNumber is null)
            {
                return;
            }
            var order = await _orderRepository.GetAsync(trackingNumber) ?? throw new OrderNotFoundException(trackingNumber);
            order.Complete();
            await _orderRepository.UpdateAsync();
            _logger.LogInformation("Order's: {order} was changed to completed by InPost webhook.", order);
        }
    }
}
