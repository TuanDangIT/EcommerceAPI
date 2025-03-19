using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Modules.Orders.Domain.Returns.Events;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Events.Externals
{
    internal class ReturnProductAddedHandler : IDomainEventHandler<ReturnProductAdded>
    {
        private readonly IOrderRepository _orderRepository;

        public ReturnProductAddedHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task HandleAsync(ReturnProductAdded @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId, default,
                query => query.Include(o => o.Products)) ?? throw new OrderNotFoundException(@event.OrderId);
            order.DecreaseProductQuantity(@event.SKU, @event.Quantity);
            await _orderRepository.UpdateAsync();
        }
    }
}
