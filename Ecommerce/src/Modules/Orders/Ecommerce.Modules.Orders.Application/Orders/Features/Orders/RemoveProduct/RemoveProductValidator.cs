using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.RemoveProduct
{
    internal class RemoveProductValidator : AbstractValidator<RemoveProduct>
    {
        public RemoveProductValidator()
        {
            RuleFor(r => r.OrderId)
                .NotEmpty()
                .NotNull();
            RuleFor(r => r.ProductId)
                .NotEmpty()
                .NotNull();
            RuleFor(r => r.Quantity)
                .GreaterThan(0);
        }
    }
}
