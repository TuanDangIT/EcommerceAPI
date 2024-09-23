using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Complaints.Events.Handlers
{
    internal sealed class ComplaintSubmittedHandler : IDomainEventHandler<ComplaintSubmitted>
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IOrderRepository _orderRepository;

        public ComplaintSubmittedHandler(IComplaintRepository complaintRepository, IOrderRepository orderRepository)
        {
            _complaintRepository = complaintRepository;
            _orderRepository = orderRepository;
        }
        public async Task HandleAsync(ComplaintSubmitted @event)
        {
            var order = await _orderRepository.GetOrderAsync(@event.OrderId);
            if(order is null)
            {
                throw new OrderNotFoundException(@event.OrderId);
            }
            await _complaintRepository.CreateAsync(new Entities.Complaint(Guid.NewGuid(), order!, @event.Title, @event.Description, @event.CreatedAt));
        }
    }
}
