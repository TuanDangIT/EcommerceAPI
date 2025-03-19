using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaints.DeleteComplaint
{
    internal class DeleteComplaintValidator : AbstractValidator<DeleteComplaint>
    {
        public DeleteComplaintValidator()
        {
            RuleFor(d => d.ComplaintId)
                .NotEmpty()
                .NotNull();
        }
    }
}
