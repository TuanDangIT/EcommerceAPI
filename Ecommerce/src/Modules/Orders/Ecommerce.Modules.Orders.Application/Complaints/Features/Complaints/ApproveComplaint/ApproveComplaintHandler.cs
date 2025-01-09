using Ecommerce.Modules.Orders.Application.Complaints.Events;
using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Application.Shared.Stripe;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.ApproveComplaint
{
    internal class ApproveComplaintHandler : ICommandHandler<ApproveComplaint>
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IPaymentProcessorService _paymentProcessorService;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<ApproveComplaintHandler> _logger;
        private readonly IContextService _contextService;

        public ApproveComplaintHandler(IComplaintRepository complaintRepository, IPaymentProcessorService paymentProcessorService, 
            IMessageBroker messageBroker, ILogger<ApproveComplaintHandler> logger, IContextService contextService)
        {
            _complaintRepository = complaintRepository;
            _paymentProcessorService = paymentProcessorService;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(ApproveComplaint request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetAsync(request.ComplaintId, cancellationToken,
                query => query.Include(c => c.Order)) ?? 
                throw new ComplaintNotFoundException(request.ComplaintId);
            if (request.Decision.RefundAmount is not null)
            {
                complaint.Approve(new Domain.Complaints.Entities.Decision(request.Decision.DecisionText, 
                    request.Decision.AdditionalInformation, (decimal)request.Decision.RefundAmount));
            }
            else
            {
                complaint.Approve(new Domain.Complaints.Entities.Decision(request.Decision.DecisionText, 
                    request.Decision.AdditionalInformation));
            }
            await _complaintRepository.UpdateAsync(cancellationToken);
            if(request.Decision.RefundAmount is null)
            {
                await _paymentProcessorService.RefundAsync(complaint.Order);
            }
            else
            {
                if(request.Decision.RefundAmount <= 0 || request.Decision.RefundAmount > complaint.Order.Products.Sum(p => p.Price))
                {
                    throw new ComplaintInvalidAmountToReturnException();
                }
                await _paymentProcessorService.RefundAsync(complaint.Order, (decimal)request.Decision.RefundAmount);
            }
            _logger.LogInformation("Complaint: {complaintId} was approved by {@user}.", complaint.Id, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            await _messageBroker.PublishAsync(new ComplaintApproved(
                complaint.Id,
                complaint.OrderId,
                complaint.Order.Customer.UserId,
                complaint.Order.Customer.FirstName,
                complaint.Order.Customer.Email,
                complaint.Title,
                complaint.Decision!.DecisionText,
                complaint.Decision.AdditionalInformation,
                request.Decision.RefundAmount,
                complaint.CreatedAt));
        }
    }
}
