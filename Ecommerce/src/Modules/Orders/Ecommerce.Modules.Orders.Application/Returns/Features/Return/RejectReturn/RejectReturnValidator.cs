using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.RejectReturn
{
    internal class RejectReturnValidator : AbstractValidator<RejectReturn>
    {
        public RejectReturnValidator()
        {
            RuleFor(r => r.ReturnId)
                .NotEmpty()
                .NotNull();
            RuleFor(r => r.RejectReason)
                .NotEmpty()
                .NotNull();
        }
    }
}
