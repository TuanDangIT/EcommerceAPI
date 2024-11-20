using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ChangeProductPrice
{
    internal class ChangeProductPriceValidator : AbstractValidator<ChangeProductPrice>
    {
        public ChangeProductPriceValidator()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.Price)
                .NotEmpty()
                .NotNull()
                .GreaterThanOrEqualTo(0);
        }
    }
}
