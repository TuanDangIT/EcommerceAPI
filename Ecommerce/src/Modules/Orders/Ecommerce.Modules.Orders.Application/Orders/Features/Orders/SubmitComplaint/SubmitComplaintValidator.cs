using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.SubmitComplaint
{
    internal class SubmitComplaintValidator : AbstractValidator<SubmitComplaint>
    {
        public SubmitComplaintValidator()
        {
            RuleFor(s => s.Title)
                .MinimumLength(2)
                .MaximumLength(32)
                .NotEmpty()
                .NotNull();
            RuleFor(s => s.Description)
                .NotEmpty()
                .NotNull();
            RuleFor(s => s.OrderId)
                .NotEmpty()
                .NotNull();
        }
    }
}
