using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects;
using System.Reflection.Emit;
using Ecommerce.Modules.Orders.Application.Orders.Services;
using Microsoft.Extensions.Logging;
using Ecommerce.Shared.Abstractions.Contexts;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.CreateShipment
{
    internal class CreateShipmentHandler : ICommandHandler<CreateShipment>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IDeliveryService _deliveryService;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<CreateShipmentHandler> _logger;
        private readonly IContextService _contextService;

        public CreateShipmentHandler(IOrderRepository orderRepository, IShipmentRepository shipmentRepository,
            IDeliveryService deliveryService, TimeProvider timeProvider, ILogger<CreateShipmentHandler> logger, IContextService contextService)
        {
            _orderRepository = orderRepository;
            _shipmentRepository = shipmentRepository;
            _deliveryService = deliveryService;
            _timeProvider = timeProvider;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(CreateShipment request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken) ?? 
                throw new OrderNotFoundException(request.OrderId);
            var receiver = new Receiver(order.Customer.FirstName, order.Customer.LastName, order.Customer.Email, order.Customer.PhoneNumber, order.Customer.Address);
            var parcels = request.Parcels.Select(p =>
            {
                var dimensions = new Dimensions(p.Length, p.Width, p.Height);
                var weight = new Weight(p.Weight);
                var parcel = new Parcel(dimensions, weight);
                return parcel;
            });
            var shipment = new Domain.Orders.Entities.Shipment(receiver, parcels, order.TotalSum);
            order.AddShipment(shipment);
            var (labelId, trackingNumber) = await _deliveryService.CreateShipmentAsync(shipment);
            shipment.SetLabelId(labelId.ToString());
            shipment.SetTrackingNumber(trackingNumber);
            shipment.SetLabelCreatedAt(_timeProvider.GetUtcNow().UtcDateTime);
            order.Pack();
            await _orderRepository.UpdateAsync();
            _logger.LogInformation("Shipment was created for order: {order} for {user}.", order, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
