using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.DeleteCategory
{
    internal class DeleteCategoryValidator : AbstractValidator<DeleteCategory>
    {
        public DeleteCategoryValidator()
        {
            RuleFor(d => d.CategoryId)
                .NotEmpty()
                .NotNull();
        }
    }
}
