using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.EditDecision
{
    internal sealed class EditDecisionHandler : ICommandHandler<EditDecision>
    {
        private readonly IComplaintRepository _complaintRepository;

        public EditDecisionHandler(IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
        }
        public async Task Handle(EditDecision request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetAsync(request.ComplaintId);
            if (complaint is null)
            {
                throw new ComplaintNotFoundException(request.ComplaintId);
            }
            if (complaint.Decision is not null && complaint.Decision.RefundAmount is null && request.Decision.RefundAmount is not null)
            {
                throw new ComplaintCannotEditRefundAmountException();
            }
            complaint.EditDecision(new Domain.Complaints.Entities.Decision(request.Decision.DecisionText, request.Decision.AdditionalInformation));
            await _complaintRepository.UpdateAsync();
        }
    }
}
