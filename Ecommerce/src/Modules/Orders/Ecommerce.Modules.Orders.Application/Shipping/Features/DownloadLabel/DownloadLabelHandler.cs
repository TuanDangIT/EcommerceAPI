using Ecommerce.Modules.Orders.Application.Delivery;
using Ecommerce.Modules.Orders.Application.Shipping.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Shipping.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Shipping.Features.DownloadLabel
{
    internal class DownloadLabelHandler : ICommandHandler<DownloadLabel, (Stream FileStream, string MimeType, string FileName)>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IDeliveryService _deliveryService;

        public DownloadLabelHandler(IShipmentRepository shipmentRepository, IDeliveryService deliveryService)
        {
            _shipmentRepository = shipmentRepository;
            _deliveryService = deliveryService;
        }
        public async Task<(Stream FileStream, string MimeType, string FileName)> Handle(DownloadLabel request, CancellationToken cancellationToken)
        {
            var shipment = await _shipmentRepository.GetAsync(request.ShipmentId) ?? throw new ShipmentNotFoundException(request.ShipmentId);
            var file = await _deliveryService.GetLabelAsync(shipment);
            return file;
        }
    }
}
