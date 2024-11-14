using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ChangeProductQuantity
{
    internal class ChangeProductQuantityValidator : AbstractValidator<ChangeProductQuantity>
    {
        public ChangeProductQuantityValidator()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.Quantity)
                .NotEmpty()
                .NotNull()
                .GreaterThanOrEqualTo(0);
        }
    }
}
