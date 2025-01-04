using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.EditProductUnitPrice
{
    internal class EditProductUnitPriceValidator : AbstractValidator<EditProductUnitPrice>
    {
        public EditProductUnitPriceValidator()
        {
            RuleFor(e => e.OrderId)
                .NotEmpty()
                .NotNull();
            RuleFor(e => e.ProductId)
                .NotEmpty()
                .NotNull();
            RuleFor(e => e.UnitPrice)
                .NotEmpty()
                .NotNull()
                .GreaterThanOrEqualTo(0);
        }
    }
}
