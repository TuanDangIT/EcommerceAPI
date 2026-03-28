using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.RemoveOrderItem
{
    internal class RemoveOrderItemValidator : AbstractValidator<RemoveOrderItem>
    {
        public RemoveOrderItemValidator()
        {
            RuleFor(r => r.OrderId)
                .NotEmpty()
                .NotNull();
            RuleFor(r => r.OrderItemId)
                .NotEmpty()
                .NotNull();
            RuleFor(r => r.Quantity)
                .GreaterThan(0);
        }
    }
}
