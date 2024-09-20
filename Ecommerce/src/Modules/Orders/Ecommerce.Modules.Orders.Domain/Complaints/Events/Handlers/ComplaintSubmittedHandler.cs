using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Modules.Orders.Domain.Orders.Events;
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

        public ComplaintSubmittedHandler(IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
        }
        public async Task HandleAsync(ComplaintSubmitted @event)
            => await _complaintRepository.CreateComplaintAsync(new Entities.Complaint(Guid.NewGuid(), @event.Customer, @event.Order, @event.Title, @event.Description, @event.CreatedAt));
    }
}
