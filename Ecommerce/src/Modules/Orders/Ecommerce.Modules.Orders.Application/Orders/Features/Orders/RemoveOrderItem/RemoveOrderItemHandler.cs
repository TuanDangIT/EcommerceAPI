using Ecommerce.Modules.Orders.Application.Orders.Events;
using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.RemoveOrderItem
{
    internal class RemoveProductHandler : ICommandHandler<RemoveOrderItem>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<RemoveProductHandler> _logger;
        private readonly IContextService _contextService;

        public RemoveProductHandler(IOrderRepository orderRepository, IProductRepository productRepository, IMessageBroker messageBroker, ILogger<RemoveProductHandler> logger,
            IContextService contextService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(RemoveOrderItem request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken,
                query => query.Include(o => o.Products)) ??
                throw new OrderNotFoundException(request.OrderId);

            var orderItem = order.Products.FirstOrDefault(oi => oi.Id == request.OrderItemId) ??
                throw new OrderItemNotFoundException(request.OrderId, request.OrderItemId);

            var product = await _productRepository.GetAsync(orderItem.SKU, cancellationToken) ??
                throw new ProductNotFoundException(orderItem.SKU);

            order.RemoveProduct(orderItem, request.Quantity);
           
            if (request.Quantity is not null)
            {
                product.IncreaseQuantity((int)request.Quantity!);
            }

            await _orderRepository.UpdateAsync(cancellationToken);

            await _messageBroker.PublishAsync(new ProductRemovedFromOrder(product.Id, request.Quantity));

            _logger.LogInformation("Order item: {orderItemId} was removed from order: {orderId} by {@user}.", 
                request.OrderItemId, order.Id, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
