using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Events.Handlers
{
    internal class ReturnHandledHandler : IDomainEventHandler<ReturnHandled>
    {
        private readonly IOrderRepository _orderRepository;

        public ReturnHandledHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task HandleAsync(ReturnHandled @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId) ?? throw new OrderNotFoundException(@event.OrderId);

            var totalRefund = @event.Products.Sum(p => p.Price * p.Quantity);
            order.SetTotalPaidSum(order.TotalPaidSum - totalRefund);

            await _orderRepository.UpdateAsync();
        }
    }
}
