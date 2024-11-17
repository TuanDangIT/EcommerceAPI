using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.RejectComplaint
{
    internal class RejectComplaintValidator : AbstractValidator<RejectComplaint>
    {
        public RejectComplaintValidator()
        {
            RuleFor(r => r.ComplaintId)
                .NotNull()
                .NotEmpty();
            RuleFor(r => r.Decision)
                .NotEmpty()
                .NotNull();
                //.ChildRules(d =>
                //{
                //    d.RuleFor(d => d.RefundAmount)
                //        .Null();
                //});
        }
    }
}
