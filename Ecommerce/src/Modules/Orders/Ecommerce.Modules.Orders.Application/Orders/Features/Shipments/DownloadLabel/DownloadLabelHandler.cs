using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.DownloadLabel
{
    internal class DownloadLabelHandler : IQueryHandler<DownloadLabel, (Stream FileStream, string MimeType, string FileName)>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryServiceFactory _deliveryServiceFactory;

        //private readonly IDeliveryService _deliveryService;
        private readonly ILogger<DownloadLabelHandler> _logger;
        private readonly IContextService _contextService;

        public DownloadLabelHandler(IOrderRepository orderRepository, /*IDeliveryService deliveryService*/ IDeliveryServiceFactory deliveryServiceFactory, ILogger<DownloadLabelHandler> logger,
            IContextService contextService)
        {
            _orderRepository = orderRepository;
            _deliveryServiceFactory = deliveryServiceFactory;
            //_deliveryService = deliveryService;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task<(Stream FileStream, string MimeType, string FileName)> Handle(DownloadLabel request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken,
                query => query.Include(o => o.Shipments), query => query.Include(o => o.DeliveryService)) ??
                throw new OrderNotFoundException(request.OrderId);
            var deliveryService = _deliveryServiceFactory.GetDeliveryService(order.DeliveryService.Courier) ??
                throw new ShipmentCourierNotSupportedException(order.DeliveryService.Courier);
            var shipment = order.Shipments.SingleOrDefault(s => s.Id == request.ShipmentId) ??
                throw new ShipmentNotFoundException(request.OrderId, request.ShipmentId);
            if (!shipment.HasTrackingNumber)
            {
                throw new LabelNotCreatedException(request.OrderId, request.ShipmentId);
            }
            var file = await deliveryService.GetLabelAsync(shipment, cancellationToken);
            _logger.LogInformation("Label: {trackingNumber} was downloaded for order: {orderId} by {@user}.", shipment.TrackingNumber,
                order.Id, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            return file;
        }
    }
}
