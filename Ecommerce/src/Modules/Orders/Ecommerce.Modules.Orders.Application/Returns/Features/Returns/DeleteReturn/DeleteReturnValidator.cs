using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.DeleteReturn
{
    internal class DeleteReturnValidator : AbstractValidator<DeleteReturn>
    {
        public DeleteReturnValidator()
        {
            RuleFor(d => d.ReturnId)
                .NotEmpty()
                .NotNull();
        }
    }
}
