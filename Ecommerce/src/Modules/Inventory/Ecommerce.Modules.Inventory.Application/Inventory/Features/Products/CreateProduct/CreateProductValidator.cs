using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.CreateProduct
{
    internal sealed class CreateProductValidator : AbstractValidator<CreateProduct>
    {
        public CreateProductValidator()
        {
            RuleFor(c => c.SKU)
                .NotEmpty()
                .NotNull()
                .MinimumLength(8)
                .MaximumLength(16);
            RuleFor(c => c.EAN)
                .Must(e => e?.Length == 8 || e?.Length == 13 || e == null)
                .WithMessage("EAN must be either 8 or 13 characters long.");
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(24);
            RuleFor(c => c.Price)
                .NotNull()
                .PrecisionScale(11, 2, false)
                .GreaterThanOrEqualTo(0);
            RuleFor(c => c.VAT)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100);
            RuleFor(c => c.Location)
                .MaximumLength(64);
            RuleFor(c => c.Quantity)
                .GreaterThanOrEqualTo(0);
            RuleFor(c => c.Description)
                .NotEmpty()
                .NotNull();
            RuleForEach(c => c.ProductParameters)
                .ChildRules(p =>
                {
                    p.RuleFor(p => p.ParameterId)
                        .NotNull()
                        .NotEmpty();
                    p.RuleFor(p => p.Value)
                        .NotNull()
                        .NotEmpty();
                });
        }
    }
}
