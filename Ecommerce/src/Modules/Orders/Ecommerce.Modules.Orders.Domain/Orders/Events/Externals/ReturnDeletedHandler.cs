using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
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
    internal class ReturnDeletedHandler : IDomainEventHandler<ReturnDeleted>
    {
        private readonly IOrderRepository _orderRepository;

        public ReturnDeletedHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task HandleAsync(ReturnDeleted @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId, default,
                query => query.Include(o => o.Products)) ?? throw new OrderNotFoundException(@event.OrderId);
            foreach(var product in @event.Products)
            {
                order.AddProduct(product.SKU, product.Quantity);
            }
            order.ChangeStatus(OrderStatus.Completed);
            await _orderRepository.UpdateAsync();
        }
    }
}
