using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EditDecisionHandler> _logger;
        private readonly IContextService _contextService;

        public EditDecisionHandler(IComplaintRepository complaintRepository, ILogger<EditDecisionHandler> logger, IContextService contextService)
        {
            _complaintRepository = complaintRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(EditDecision request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetAsync(request.ComplaintId, cancellationToken) ?? 
                throw new ComplaintNotFoundException(request.ComplaintId);
            if (complaint.Decision is not null && complaint.Decision.RefundAmount is null && request.Decision.RefundAmount is not null)
            {
                throw new ComplaintCannotEditRefundAmountException();
            }
            complaint.EditDecision(new Domain.Complaints.Entities.Decision(request.Decision.DecisionText, request.Decision.AdditionalInformation));
            await _complaintRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Decision: {@decision} was edited for complaint: {@complaint} with new details: {@updatingDetails} by {@user}.", 
                complaint.Decision, complaint, request.Decision, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
