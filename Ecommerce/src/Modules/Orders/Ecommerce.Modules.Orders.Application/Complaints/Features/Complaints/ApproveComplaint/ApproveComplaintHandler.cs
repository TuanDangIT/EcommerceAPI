using Ecommerce.Modules.Orders.Application.Complaints.Events;
using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Application.Shared.Stripe;
using Ecommerce.Modules.Orders.Domain.Complaints.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.DomainEvents;
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
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public ApproveComplaintHandler(IComplaintRepository complaintRepository, IPaymentProcessorService paymentProcessorService, 
            IMessageBroker messageBroker, ILogger<ApproveComplaintHandler> logger, IContextService contextService, IDomainEventDispatcher domainEventDispatcher)
        {
            _complaintRepository = complaintRepository;
            _paymentProcessorService = paymentProcessorService;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
            _domainEventDispatcher = domainEventDispatcher;
        }
        public async Task Handle(ApproveComplaint request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetAsync(request.ComplaintId, cancellationToken,
                query => query.Include(c => c.Order).ThenInclude(o => o.Customer)) ?? 
                throw new ComplaintNotFoundException(request.ComplaintId);
            var orderTotal = complaint.Order.Products.Sum(p => p.Price) + complaint.Order.DeliveryService!.Price;
            if (request.Decision.RefundAmount is not null)
            {
                if (request.Decision.RefundAmount <= 0 || request.Decision.RefundAmount > complaint.Order.Products.Sum(p => p.Price))
                {
                    throw new ComplaintInvalidAmountToReturnException();
                }
                complaint.Approve(new Domain.Complaints.Entities.Decision(complaint, request.Decision.DecisionText, 
                    request.Decision.AdditionalInformation, (decimal)request.Decision.RefundAmount));
                await _paymentProcessorService.RefundAsync(complaint.Order, (decimal)request.Decision.RefundAmount + complaint.Order.DeliveryService!.Price);
            }
            else
            {
                complaint.Approve(new Domain.Complaints.Entities.Decision(complaint, request.Decision.DecisionText, 
                    request.Decision.AdditionalInformation, orderTotal));
                await _paymentProcessorService.RefundAsync(complaint.Order);
            }
            await _complaintRepository.UpdateAsync(cancellationToken);
            
            var refundAmount = request.Decision.RefundAmount ?? orderTotal;
            
            _logger.LogInformation("Complaint: {complaintId} with refund amount: {refundAmount} was approved by {@user}.", complaint.Id, refundAmount,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });

            await _domainEventDispatcher.DispatchAsync(new Domain.Complaints.Events.ComplaintApproved(complaint.Id, refundAmount));

            await _messageBroker.PublishAsync(new ComplaintApproved(
                complaint.Id,
                complaint.OrderId,
                complaint.Order.Customer!.UserId,
                complaint.Order.Customer.FirstName,
                complaint.Order.Customer.Email,
                complaint.Title,
                complaint.Decision!.DecisionText,
                complaint.Decision.AdditionalInformation,
                refundAmount,
                complaint.CreatedAt));
        }
    }
}
