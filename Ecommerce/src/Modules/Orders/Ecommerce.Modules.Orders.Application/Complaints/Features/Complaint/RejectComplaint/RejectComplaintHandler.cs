using Ecommerce.Modules.Orders.Application.Complaints.Events;
using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.RejectComplaint
{
    internal class RejectComplaintHandler : ICommandHandler<RejectComplaint>
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly TimeProvider _timeProvider;

        public RejectComplaintHandler(IComplaintRepository complaintRepository, IMessageBroker messageBroker, TimeProvider timeProvider)
        {
            _complaintRepository = complaintRepository;
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
        }
        public async Task Handle(RejectComplaint request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetAsync(request.ComplaintId);
            if (complaint is null)
            {
                throw new ComplaintNotFoundException(request.ComplaintId);
            }
            complaint.Reject(_timeProvider.GetUtcNow().UtcDateTime);
            await _complaintRepository.UpdateAsync();
            //More logic here: Mail noty
            await _messageBroker.PublishAsync(new ComplaintRejected(
                complaint.Id,
                complaint.OrderId,
                complaint.Order.Customer.UserId,
                complaint.Order.Customer.FirstName,
                complaint.Order.Customer.Email,
                complaint.Title,
                complaint.Decision!.DecisionText,
                complaint.Decision.AdditionalInformation,
                complaint.CreatedAt));
        }
    }
}
