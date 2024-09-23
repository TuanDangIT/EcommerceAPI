using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Exception;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Events.Handlers
{
    internal sealed class OrderReturnedHandler : IDomainEventHandler<OrderReturned>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderReturnedHandler(IReturnRepository returnRepository, IOrderRepository orderRepository)
        {
            _returnRepository = returnRepository;
            _orderRepository = orderRepository;
        }
        public async Task HandleAsync(OrderReturned @event)
        {
            var order = await _orderRepository.GetOrderAsync(@event.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(@event.OrderId);
            }
            var @return = await _returnRepository.GetByOrderIdAsync(@event.OrderId);
            if(@return is not null)
            {
                throw new ReturnCreateForTheSameOrderException(@event.OrderId);
            }
            await _returnRepository.CreateAsync(new Return(
                    Guid.NewGuid(),
                    order,
                    @event.Products.Select(p => new ReturnProduct(p.SKU, p.Name, p.Price, p.Quantity, p.ImagePathUrl)),
                    @event.ReasonForReturn,
                    @event.IsFullReturn,
                    @event.CreatedAt
                ));
        }
    }
}
