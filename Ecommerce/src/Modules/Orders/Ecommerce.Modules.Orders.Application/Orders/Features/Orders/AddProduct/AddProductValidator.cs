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
            RuleFor(a => a.ProductId)
                .NotEmpty()
                .NotNull();
            RuleFor(a => a.Quantity)
                .GreaterThan(0)
                .NotEmpty()
                .NotNull();
        }
    }
}
