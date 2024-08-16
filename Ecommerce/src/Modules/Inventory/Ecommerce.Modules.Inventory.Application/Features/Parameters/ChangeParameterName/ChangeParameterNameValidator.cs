using Ecommerce.Modules.Inventory.Application.Features.Category.ChangeCategoryName;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.ChangeParameterName
{
    internal class ChangeParameterNameValidator : AbstractValidator<ChangeParameterName>
    {
        public ChangeParameterNameValidator()
        {
            RuleFor(c => c.ParameterId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull();
        }
    }
}
