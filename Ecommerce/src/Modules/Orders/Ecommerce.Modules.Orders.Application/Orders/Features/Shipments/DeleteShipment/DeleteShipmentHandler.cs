using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
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

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.DeleteShipment
{
    internal class DeleteShipmentHandler : ICommandHandler<DeleteShipment>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteShipmentHandler> _logger;
        private readonly IContextService _contextService;

        public DeleteShipmentHandler(IOrderRepository orderRepository, ILogger<DeleteShipmentHandler> logger, IContextService contextService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteShipment request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken,
                query => query.Include(o => o.Shipments)) ?? 
                throw new OrderNotFoundException(request.OrderId);
            order.DeleteShipment(request.ShipmentId);
            await _orderRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Shipment: {shipmentId} was deleted for {orderId} by {@user}.", request.ShipmentId, order.Id,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
