using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.RemoveProduct
{
    internal class RemoveProductHandler : ICommandHandler<RemoveProduct>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<RemoveProductHandler> _logger;
        private readonly IContextService _contextService;

        public RemoveProductHandler(IOrderRepository orderRepository, ILogger<RemoveProductHandler> logger,
            IContextService contextService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(RemoveProduct request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken) ??
                throw new OrderNotFoundException(request.OrderId);
            order.RemoveProduct(request.ProductId, request.Quantity);
            await _orderRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Product: {productId} was removed from order: {orderId} by {@user}.", 
                request.ProductId, order.Id, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
