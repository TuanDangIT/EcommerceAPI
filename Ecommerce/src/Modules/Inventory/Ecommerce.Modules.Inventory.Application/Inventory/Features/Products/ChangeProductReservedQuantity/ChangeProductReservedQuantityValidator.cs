using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ChangeProductReservedQuantity
{
    internal class ChangeProductReservedQuantityValidator : AbstractValidator<ChangeProductReservedQuantity>
    {
        public ChangeProductReservedQuantityValidator()
        {
            RuleFor(c => c.ProductId)
                .NotNull()
                .NotEmpty();
            RuleFor(c => c.Reserved)
                .NotNull()
                .GreaterThanOrEqualTo(0);
        }
    }
}
