using Ecommerce.Modules.Orders.Application.Delivery;
using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.DownloadLabel
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
            var order = await _orderRepository.GetOrderAsync(request.OrderId) ?? throw new OrderNotFoundException(request.OrderId);
            var file = await _deliveryService.GetLabelAsync(order.Shipment);
            return file;
        }
    }
}
