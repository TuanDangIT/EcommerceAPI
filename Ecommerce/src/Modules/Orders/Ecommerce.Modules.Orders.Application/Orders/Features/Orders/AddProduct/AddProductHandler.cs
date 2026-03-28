using Ecommerce.Modules.Orders.Application.Orders.Events;
using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
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

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.AddProduct
{
    internal class AddProductHandler : ICommandHandler<AddProduct>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<AddProductHandler> _logger;
        private readonly IContextService _contextService;

        public AddProductHandler(IOrderRepository orderRepository, IProductRepository productRepository, IMessageBroker messageBroker, ILogger<AddProductHandler> logger, IContextService contextService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(AddProduct request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId,cancellationToken,
                query => query.Include(o => o.Products)) ??
                throw new OrderNotFoundException(request.OrderId);

            var product = await _productRepository.GetAsync(request.ProductId, cancellationToken) ??
                throw new ProductNotFoundException(request.ProductId);

            product.DecreaseQuantity(request.Quantity);

            var orderItem = order.Products.FirstOrDefault(p => p.SKU == product.SKU);

            if (orderItem is not null)
            {
                order.AddProduct(orderItem.SKU, request.Quantity);
            }
            else
            {
                order.AddProduct(product, request.Quantity);
            }

            await _orderRepository.UpdateAsync(cancellationToken);

            await _messageBroker.PublishAsync(new ProductAddedToOrder(product.Id, request.Quantity));

            _logger.LogInformation("Product: {product} was added to order: {orderId} by {@user}.", new { request.ProductId, request.Quantity },
                order.Id, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
