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
                .Length(13);
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(24);
            RuleFor(c => c.Price)
                .NotEmpty()
                .NotNull()
                .PrecisionScale(11, 2, false)
                .GreaterThanOrEqualTo(0);
            RuleFor(c => c.VAT)
                .NotEmpty()
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
            RuleFor(c => c.ManufacturerId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.CategoryId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.Images)
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
                //.When(c => c.ProductParameters is not null);

        }
    }
}
