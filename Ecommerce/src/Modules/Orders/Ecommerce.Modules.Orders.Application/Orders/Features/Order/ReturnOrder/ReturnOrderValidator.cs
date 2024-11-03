using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.ReturnOrder
{
    internal sealed class ReturnOrderValidator : AbstractValidator<ReturnOrder>
    {
        public ReturnOrderValidator()
        {
            RuleFor(r => r.ReasonForReturn)
                .MinimumLength(2)
                .MaximumLength(32)
                .NotEmpty()
                .NotNull();
            RuleFor(r => r.ProductsToReturn)
                .NotEmpty()
                .NotNull();
            RuleForEach(r => r.ProductsToReturn)
                .ChildRules(ptr =>
                {
                    ptr.RuleFor(ptr => ptr.SKU)
                        .NotEmpty()
                        .NotNull();
                    ptr.RuleFor(ptr => ptr.Quantity)
                        .GreaterThan(0)
                        .NotEmpty()
                        .NotNull();
                });
            RuleFor(r => r.OrderId)
                .NotEmpty()
                .NotNull();
        }
    }
}
