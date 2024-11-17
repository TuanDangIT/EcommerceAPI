using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.DownloadLabel
{
    internal class DownloadLabelHandler : ICommandHandler<DownloadLabel, (Stream FileStream, string MimeType, string FileName)>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryService _deliveryService;

        public DownloadLabelHandler(IOrderRepository orderRepository, IDeliveryService deliveryService)
        {
            _orderRepository = orderRepository;
            _deliveryService = deliveryService;
        }
        public async Task<(Stream FileStream, string MimeType, string FileName)> Handle(DownloadLabel request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId) ?? throw new OrderNotFoundException(request.OrderId);
            var shipment = order.Shipments.SingleOrDefault(s => s.Id == request.ShipmentId);
            if (shipment is null)
            {
                throw new ShipmentNotFoundException(request.ShipmentId);
            }
            if (shipment.TrackingNumber is null)
            {
                throw new LabelNotCreatedException(request.ShipmentId);
            }
            var file = await _deliveryService.GetLabelAsync(shipment);
            return file;
        }
    }
}
