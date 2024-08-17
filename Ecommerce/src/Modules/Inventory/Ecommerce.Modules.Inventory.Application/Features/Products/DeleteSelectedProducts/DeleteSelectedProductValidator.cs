using Ecommerce.Modules.Inventory.Application.Features.Category.ChangeCategoryName;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.DeleteSelectedProducts
{
    internal sealed class DeleteSelectedProductValidator : AbstractValidator<DeleteSelectedProducts>
    {
        public DeleteSelectedProductValidator()
        {
            RuleFor(d => d.ProductIds)
                .NotEmpty()
                .NotNull();
        }
    }
}
