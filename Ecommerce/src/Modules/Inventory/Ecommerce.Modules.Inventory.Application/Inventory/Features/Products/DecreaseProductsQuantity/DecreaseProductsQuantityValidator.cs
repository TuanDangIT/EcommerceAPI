using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.DecreaseProductsQuantity
{
    internal sealed class DecreaseProductsQuantityValidator : AbstractValidator<DecreaseProductsQuantity>
    {
        public DecreaseProductsQuantityValidator()
        {
            RuleFor(d => d.ProductId)
                .NotEmpty()
                .NotNull();
            RuleFor(d => d.Quantity)
                .GreaterThan(0)
                .NotEmpty()
                .NotNull();
        }
    }
}
