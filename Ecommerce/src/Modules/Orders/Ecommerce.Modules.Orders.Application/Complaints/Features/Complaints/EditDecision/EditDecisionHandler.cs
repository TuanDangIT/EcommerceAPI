using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Complaints.Entities.Enums;
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
            complaint.EditDecision(request.DecisionText, request.DecisionAdditionalInformation);
            await _complaintRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Decision was edited for complaint: {complaintId} by {@user}.", 
                complaint.Id, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
