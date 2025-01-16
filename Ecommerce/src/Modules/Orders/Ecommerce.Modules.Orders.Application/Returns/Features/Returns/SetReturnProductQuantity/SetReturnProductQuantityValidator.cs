using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.SetReturnProductQuantity
{
    internal class SetReturnProductQuantityValidator : AbstractValidator<SetReturnProductQuantity>
    {
        public SetReturnProductQuantityValidator()
        {
            RuleFor(c => c.ReturnId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.ProductId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.ProductId)
                .NotNull()
                .GreaterThanOrEqualTo(0);
        }
    }
}
