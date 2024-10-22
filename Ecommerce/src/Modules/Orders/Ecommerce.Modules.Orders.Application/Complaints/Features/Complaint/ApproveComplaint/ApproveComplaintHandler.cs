using Ecommerce.Modules.Orders.Application.Complaints.Events;
using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Application.Stripe;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
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
        private readonly IStripeService _stripeService;
        private readonly IMessageBroker _messageBroker;
        private readonly TimeProvider _timeProvider;

        public ApproveComplaintHandler(IComplaintRepository complaintRepository, IStripeService stripeService, IMessageBroker messageBroker, TimeProvider timeProvider)
        {
            _complaintRepository = complaintRepository;
            _stripeService = stripeService;
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
        }
        public async Task Handle(ApproveComplaint request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetAsync(request.ComplaintId);
            if (complaint is null)
            {
                throw new ComplaintNotFoundException(request.ComplaintId);
            }
            complaint.WriteDecision(new Domain.Complaints.Entities.Decision(request.Decision.DecisionText, request.Decision.AdditionalInformation, request.Amount));
            if(request.Amount is null)
            {
                await _stripeService.Refund(complaint.Order);
            }
            else
            {
                if(request.Amount <= 0 || request.Amount > complaint.Order.Products.Sum(p => p.Price))
                {
                    throw new ComplaintInvalidAmountToReturnException();
                }
                await _stripeService.Refund(complaint.Order, (decimal)request.Amount);
            }
            complaint.Approve(_timeProvider.GetUtcNow().UtcDateTime);
            await _complaintRepository.UpdateAsync();
            await _messageBroker.PublishAsync(new ComplaintApproved(
                complaint.Id,
                complaint.OrderId,
                complaint.Order.Customer.UserId,
                complaint.Order.Customer.FirstName,
                complaint.Order.Customer.Email,
                complaint.Title,
                complaint.Decision!.DecisionText,
                complaint.Decision.AdditionalInformation,
                request.Amount,
                complaint.CreatedAt));
        }
    }
}
