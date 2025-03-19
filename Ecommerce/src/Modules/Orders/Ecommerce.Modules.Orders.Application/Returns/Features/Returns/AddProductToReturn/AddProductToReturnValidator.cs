using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.AddProductToReturn
{
    internal class AddProductToReturnValidator : AbstractValidator<AddProductToReturn>
    {
        public AddProductToReturnValidator()
        {
            RuleFor(a => a.ReturnId)
                .NotNull()
                .NotEmpty();
            RuleFor(a => a.SKU)
                .NotNull()
                .NotEmpty();
            RuleFor(a => a.Quantity)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
