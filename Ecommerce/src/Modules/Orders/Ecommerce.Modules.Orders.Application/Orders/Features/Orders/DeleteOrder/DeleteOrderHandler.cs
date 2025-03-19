using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.DeleteOrder
{
    internal class DeleteOrderHandler : ICommandHandler<DeleteOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteOrderHandler> _logger;
        private readonly IContextService _contextService;

        public DeleteOrderHandler(IOrderRepository orderRepository, ILogger<DeleteOrderHandler> logger, IContextService contextService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteOrder request, CancellationToken cancellationToken)
        {
            await _orderRepository.DeleteAsync(request.OrderId, cancellationToken);
            _logger.LogInformation("Order: {orderId} was deleted by {@user}.", request.OrderId,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
