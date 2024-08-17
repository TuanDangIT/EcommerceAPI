using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Category.ChangeCategoryName
{
    internal sealed class ChangeCategoryNameValidator : AbstractValidator<ChangeCategoryName>
    {
        public ChangeCategoryNameValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(32);
        }
    }
}
