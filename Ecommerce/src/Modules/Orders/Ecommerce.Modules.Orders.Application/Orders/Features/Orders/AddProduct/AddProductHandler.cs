using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
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

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.AddProduct
{
    internal class AddProductHandler : ICommandHandler<AddProduct>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<AddProductHandler> _logger;
        private readonly IContextService _contextService;

        public AddProductHandler(IOrderRepository orderRepository, ILogger<AddProductHandler> logger, IContextService contextService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(AddProduct request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId,cancellationToken,
                query => query.Include(o => o.Products)) ??
                throw new OrderNotFoundException(request.OrderId);
            if(request.Name is null || request.UnitPrice is null)
            {
                order.AddProduct(request.ProductId, request.Quantity);
            }
            else
            {
                var product = new Product(request.SKU, request.Name, (decimal)request.UnitPrice, request.Quantity, request.ImagePathUrl);
                order.AddProduct(product);
            }
            await _orderRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Product: {product} was added to order: {orderId} by {@user}.", new { request.ProductId, request.SKU, request.Name, request.UnitPrice, request.Quantity, request.ImagePathUrl },
                order.Id, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
