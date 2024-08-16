using Ecommerce.Modules.Inventory.Application.Features.Category.ChangeCategoryName;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.DeleteSelectedParameters
{
    internal class DeleteSelectedParametersValidator : AbstractValidator<DeleteSelectedParameters>
    {
        public DeleteSelectedParametersValidator()
        {
            RuleFor(d => d.ParameterIds)
                .NotEmpty()
                .NotNull();
        }
    }
}
