using Ecommerce.Modules.Orders.Application.Delivery;
using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.CreateLabel
{
    internal class CreateLabelHandler : ICommandHandler<CreateLabel>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryService _deliveryService;
        private readonly TimeProvider _timeProvider;

        public CreateLabelHandler(IOrderRepository orderRepository, IDeliveryService deliveryService, TimeProvider timeProvider)
        {
            _orderRepository = orderRepository;
            _deliveryService = deliveryService;
            _timeProvider = timeProvider;
        }
        public async Task Handle(CreateLabel request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderAsync(request.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            var (id, trackingNumber) = await _deliveryService.CreateShipmentAsync(order.Shipment);
            order.SetLabelDetails(trackingNumber, id.ToString(), _timeProvider.GetUtcNow().UtcDateTime);
            await _orderRepository.UpdateAsync();
        }
    }
}
