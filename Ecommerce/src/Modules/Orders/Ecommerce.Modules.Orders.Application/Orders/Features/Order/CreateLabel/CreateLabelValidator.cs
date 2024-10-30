using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.CreateLabel
{
    public class CreateLabelValidator : AbstractValidator<CreateLabel>
    {
        public CreateLabelValidator()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty()
                .NotNull();
        }
    }
}
