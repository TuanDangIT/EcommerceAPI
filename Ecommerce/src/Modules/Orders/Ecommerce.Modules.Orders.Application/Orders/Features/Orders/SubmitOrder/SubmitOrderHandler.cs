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

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.SubmitOrder
{
    internal class SubmitOrderHandler : ICommandHandler<SubmitOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<SubmitOrderHandler> _logger;
        private readonly IContextService _contextService;

        public SubmitOrderHandler(IOrderRepository orderRepository, ILogger<SubmitOrderHandler> logger,
            IContextService contextService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(SubmitOrder request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken) ??
                throw new OrderNotFoundException(request.OrderId);
            if(order.Status != OrderStatus.Placed)
            {
                throw new OrderCannotSubmitOrderException();
            }
            order.Submit();
            await _orderRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Order: {orderId} was submitted by {@user}.", order.Id,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
