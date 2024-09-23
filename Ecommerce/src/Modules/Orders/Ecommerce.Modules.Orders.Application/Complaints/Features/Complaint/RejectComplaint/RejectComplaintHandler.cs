using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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
        private readonly TimeProvider _timeProvider;

        public RejectComplaintHandler(IComplaintRepository complaintRepository, TimeProvider timeProvider)
        {
            _complaintRepository = complaintRepository;
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
        }
    }
}
