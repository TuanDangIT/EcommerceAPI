using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.ChangeStatus
{
    internal class ChangeStatusHandler : ICommandHandler<ChangeStatus>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<ChangeStatusHandler> _logger;
        private readonly IContextService _contextService;

        public ChangeStatusHandler(IOrderRepository orderRepository, ILogger<ChangeStatusHandler> logger, IContextService contextService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(ChangeStatus request, CancellationToken cancellationToken)
        {
            if(Enum.TryParse(typeof(OrderStatus), request.Status, true, out var status))
            {
                throw new OrderInvalidStatusException(request.Status);
            }
            var order = await _orderRepository.GetAsync(request.OrderId);
            if(order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            order.ChangeStatus((OrderStatus)status!);
            await _orderRepository.UpdateAsync();
            _logger.LogInformation("Order: {order} status was changed to {status} by {username}:{userId}", order, request.Status, 
                _contextService.Identity!.Username, _contextService.Identity!.Id);
        }
    }
}
