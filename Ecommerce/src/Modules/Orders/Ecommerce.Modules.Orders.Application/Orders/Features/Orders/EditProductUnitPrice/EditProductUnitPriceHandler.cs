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

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.EditProductUnitPrice
{
    internal class EditProductUnitPriceHandler : ICommandHandler<EditProductUnitPrice>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<EditProductUnitPriceHandler> _logger;
        private readonly IContextService _contextService;

        public EditProductUnitPriceHandler(IOrderRepository orderRepository, ILogger<EditProductUnitPriceHandler> logger,
            IContextService contextService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(EditProductUnitPrice request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken) ??
                throw new OrderNotFoundException(request.OrderId);
            var product = order.Products.SingleOrDefault(p => p.Id == request.ProductId) ??
                throw new ProductNotFoundException(request.OrderId, request.ProductId);
            product.EditUnitPrice(request.UnitPrice);
            await _orderRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Product's: {productId} price was updated with new price: {newPrice} for order: {orderId} by {@user}.",
                product.Id, request.UnitPrice, order.Id, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
