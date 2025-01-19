using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.HandleReturn
{
    internal class HandleReturnValidator : AbstractValidator<HandleReturn>
    {
        public HandleReturnValidator()
        {
            RuleFor(r => r.ReturnId)
                .NotNull()
                .NotEmpty();
        }
    }
}
