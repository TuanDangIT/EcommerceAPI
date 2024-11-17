using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.SetNote
{
    internal class SetNoteValidator : AbstractValidator<SetNote>
    {
        public SetNoteValidator()
        {
            RuleFor(s => s.ReturnId)
                .NotEmpty()
                .NotNull();
            RuleFor(s => s.Note)
                .NotEmpty()
                .NotNull();
        }
    }
}
