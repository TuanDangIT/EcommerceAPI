using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.SetTotalPaidSum
{
    internal class SetTotalPaidSumValidator : AbstractValidator<SetTotalPaidSum>
    {
        public SetTotalPaidSumValidator()
        {
            RuleFor(s => s.OrderId)
                .NotEmpty()
                .NotNull();

            RuleFor(s => s.PaidSum)
                .NotEmpty()
                .NotNull()
                .GreaterThanOrEqualTo(0);
        }
    }
}
