using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.SubmitOrder
{
    internal class SubmitOrderValidator : AbstractValidator<SubmitOrder>
    {
        public SubmitOrderValidator()
        {
            RuleFor(s => s.OrderId)
                .NotEmpty()
                .NotNull();
        }
    }
}
