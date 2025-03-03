using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.UpdateProduct
{
    internal sealed class UpdateProductValidator : AbstractValidator<UpdateProduct>
    {
        public UpdateProductValidator()
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
                .GreaterThanOrEqualTo(0);
            RuleFor(c => c.VAT)
                .NotNull()
                .GreaterThanOrEqualTo(0);
            RuleFor(c => c.Location)
                .MaximumLength(64);
            RuleFor(c => c.Quantity)
                .GreaterThanOrEqualTo(0);
            RuleFor(c => c.Reserved)
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
