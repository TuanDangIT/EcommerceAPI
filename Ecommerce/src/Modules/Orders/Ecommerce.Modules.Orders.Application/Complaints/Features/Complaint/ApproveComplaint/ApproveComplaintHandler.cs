﻿using Ecommerce.Modules.Orders.Application.Complaints.Events;
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

        public ApproveComplaintHandler(IComplaintRepository complaintRepository, IStripeService stripeService, IMessageBroker messageBroker)
        {
            _complaintRepository = complaintRepository;
            _stripeService = stripeService;
            _messageBroker = messageBroker;
        }
        public async Task Handle(ApproveComplaint request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetAsync(request.ComplaintId);
            if (complaint is null)
            {
                throw new ComplaintNotFoundException(request.ComplaintId);
            }
            //complaint.WriteDecision(new Domain.Complaints.Entities.Decision(request.Decision.DecisionText, request.Decision.AdditionalInformation, request.Decision.RefundAmount));
            if (request.Decision.RefundAmount is not null)
            {
                complaint.Approve(new Domain.Complaints.Entities.Decision(request.Decision.DecisionText, request.Decision.AdditionalInformation, (decimal)request.Decision.RefundAmount));
            }
            else
            {
                complaint.Approve(new Domain.Complaints.Entities.Decision(request.Decision.DecisionText, request.Decision.AdditionalInformation));
            }
            await _complaintRepository.UpdateAsync();
            if(request.Decision.RefundAmount is null)
            {
                await _stripeService.Refund(complaint.Order);
            }
            else
            {
                if(request.Decision.RefundAmount <= 0 || request.Decision.RefundAmount > complaint.Order.Products.Sum(p => p.Price))
                {
                    throw new ComplaintInvalidAmountToReturnException();
                }
                await _stripeService.Refund(complaint.Order, (decimal)request.Decision.RefundAmount);
            }
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
