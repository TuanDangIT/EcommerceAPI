using Ecommerce.Modules.Orders.Application.Complaints.Events;
using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<RejectComplaintHandler> _logger;
        private readonly IContextService _contextService;

        public RejectComplaintHandler(IComplaintRepository complaintRepository, IMessageBroker messageBroker, ILogger<RejectComplaintHandler> logger,
            IContextService contextService)
        {
            _complaintRepository = complaintRepository;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(RejectComplaint request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetAsync(request.ComplaintId);
            if (complaint is null)
            {
                throw new ComplaintNotFoundException(request.ComplaintId);
            }
            complaint.Reject(new Decision(request.Decision.DecisionText, request.Decision.AdditionalInformation));
            await _complaintRepository.UpdateAsync();
            _logger.LogInformation("Complaint: {complaint} was rejected by {username}:{userId}.", complaint, _contextService.Identity!.Username, _contextService.Identity!.Id);
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
