using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Category.DeleteSelectedCategories
{
    internal class DeleteSelectedCategoriesValidator : AbstractValidator<DeleteSelectedCategories>
    {
        public DeleteSelectedCategoriesValidator()
        {
            RuleFor(d => d.CategoryIds)
                .NotEmpty()
                .NotNull();
        }
    }
}
