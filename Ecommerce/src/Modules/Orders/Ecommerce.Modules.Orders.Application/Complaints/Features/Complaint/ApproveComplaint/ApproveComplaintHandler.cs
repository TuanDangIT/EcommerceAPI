using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Application.Stripe;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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
        private readonly TimeProvider _timeProvider;

        public ApproveComplaintHandler(IComplaintRepository complaintRepository, IStripeService stripeService, TimeProvider timeProvider)
        {
            _complaintRepository = complaintRepository;
            _stripeService = stripeService;
            _timeProvider = timeProvider;
        }
        public async Task Handle(ApproveComplaint request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetAsync(request.ComplaintId);
            if (complaint is null)
            {
                throw new ComplaintNotFoundException(request.ComplaintId);
            }
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
        }
    }
}
