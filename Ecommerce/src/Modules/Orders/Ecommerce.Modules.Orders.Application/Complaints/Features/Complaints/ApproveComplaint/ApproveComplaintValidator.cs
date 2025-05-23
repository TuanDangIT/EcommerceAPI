﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.ApproveComplaint
{
    internal class ApproveComplaintValidator : AbstractValidator<ApproveComplaint>
    {
        public ApproveComplaintValidator()
        {
            RuleFor(a => a.ComplaintId)
                .NotNull()
                .NotEmpty();
            RuleFor(a => a.Decision)
                .NotEmpty()
                .NotNull()
                .ChildRules(d =>
                {
                    d.RuleFor(d => d.RefundAmount)
                        .PrecisionScale(11, 2, true)
                        .GreaterThan(0);
                });
        }
    }
}
