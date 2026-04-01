using Ecommerce.Modules.Orders.Domain.Complaints.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Events.Externals.Handlers
{
    internal class ComplaintApprovedHandler : IDomainEventHandler<ComplaintApproved>
    {
        private readonly IOrderRepository _orderRepository;

        public ComplaintApprovedHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task HandleAsync(ComplaintApproved @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId) ?? throw new OrderNotFoundException(@event.OrderId);

            order.SetTotalPaidSum(order.TotalPaidSum - @event.RefundedAmount);

            await _orderRepository.UpdateAsync();
        }
    }
}
