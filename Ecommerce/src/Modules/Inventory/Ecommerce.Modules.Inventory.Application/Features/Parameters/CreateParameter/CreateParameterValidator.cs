using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.CreateParameter
{
    internal class CreateParameterValidator : AbstractValidator<CreateParameter>
    {
        public CreateParameterValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull();
        }
    }
}
