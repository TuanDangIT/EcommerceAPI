using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.AddProduct
{
    internal class AddProductValidator : AbstractValidator<AddProduct>
    {
        public AddProductValidator()
        {
            RuleFor(a => a.OrderId)
                .NotEmpty()
                .NotNull();
            RuleFor(a => a.SKU)
                .NotEmpty()
                .NotNull()
                .MinimumLength(8)
                .MaximumLength(16);
            RuleFor(a => a.Name)
                .MinimumLength(2)
                .MaximumLength(24);
            RuleFor(a => a.Quantity)
                .NotNull()
                .GreaterThanOrEqualTo(0);
            RuleFor(a => a.UnitPrice)
                .GreaterThanOrEqualTo(0);
        }
    }
}
