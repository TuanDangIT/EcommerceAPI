using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.CancelOrder
{
    internal class CancelOrderValidator : AbstractValidator<CancelOrder>
    {
        public CancelOrderValidator()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty()
                .NotNull();
        }
    }
}
