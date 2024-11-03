using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.SetNote
{
    internal class SetNoteValidator : AbstractValidator<SetNote>
    {
        public SetNoteValidator()
        {
            RuleFor(s => s.ComplaintId)
                .NotNull()
                .NotEmpty();
            RuleFor(s => s.Note)
                .NotNull()
                .NotEmpty();
        }
    }
}
