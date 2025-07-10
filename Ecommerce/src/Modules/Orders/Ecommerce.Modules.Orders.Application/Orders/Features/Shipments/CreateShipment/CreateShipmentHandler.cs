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
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.CreateShipment
{
    internal class CreateShipmentHandler : ICommandHandler<CreateShipment, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryServiceFactory _deliveryServiceFactory;

        //private readonly IDeliveryService _deliveryService;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<CreateShipmentHandler> _logger;
        private readonly IContextService _contextService;

        public CreateShipmentHandler(IOrderRepository orderRepository, /*IDeliveryService deliveryService*/ IDeliveryServiceFactory deliveryServiceFactory, 
            TimeProvider timeProvider, ILogger<CreateShipmentHandler> logger, IContextService contextService)
        {
            _orderRepository = orderRepository;
            _deliveryServiceFactory = deliveryServiceFactory;
            //_deliveryService = deliveryService;
            _timeProvider = timeProvider;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task<int> Handle(CreateShipment request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken,
                query => query.Include(o => o.Customer),
                query => query.Include(o => o.Shipments), 
                query => query.Include(o => o.DeliveryService)) ?? 
                throw new OrderNotFoundException(request.OrderId);
            var deliveryService = _deliveryServiceFactory.GetDeliveryService(order.DeliveryService.Courier) ?? 
                throw new ShipmentCourierNotSupportedException(order.DeliveryService.Courier);
            if (order.Status == Domain.Orders.Entities.Enums.OrderStatus.Draft)
            {
                throw new OrderDraftException(order.Id);
            }
            var receiver = new Receiver(order.Customer!.FirstName, order.Customer.LastName, order.Customer.Email, order.Customer.PhoneNumber, order.Customer.Address);
            var parcels = request.Parcels.Select(p =>
            {
                var dimensions = new Dimensions(p.Length, p.Width, p.Height);
                var weight = new Weight(p.Weight);
                var parcel = new Parcel(dimensions, weight);
                return parcel;
            });
            var shipment = new Domain.Orders.Entities.Shipment(receiver, parcels, order.TotalSum);
            order.AddShipment(shipment);
            var (labelId, trackingNumber) = await deliveryService.CreateShipmentAsync(shipment, cancellationToken);
            shipment.SetLabelId(labelId.ToString());
            shipment.SetTrackingNumber(trackingNumber);
            shipment.SetLabelCreatedAt(_timeProvider.GetUtcNow().UtcDateTime);
            order.Pack();
            await _orderRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Shipment was created for order: {orderId} for {@user}.", order.Id, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            return shipment.Id;
        }
    }
}
