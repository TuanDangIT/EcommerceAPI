using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.RemoveReturnProduct
{
    internal class RemoveReturnProductValidator : AbstractValidator<RemoveReturnProduct>
    {
        public RemoveReturnProductValidator()
        {
            RuleFor(c => c.ReturnId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.ProductId)
                .NotEmpty()
                .NotNull();
        }
    }
}
