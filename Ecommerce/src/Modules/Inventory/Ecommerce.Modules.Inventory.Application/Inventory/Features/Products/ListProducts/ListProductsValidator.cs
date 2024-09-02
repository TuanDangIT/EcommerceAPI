using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ListProducts
{
    internal sealed class ListProductsValidator : AbstractValidator<ListProducts>
    {
        public ListProductsValidator()
        {
            RuleFor(up => up.ProductIds)
                .NotNull()
                .NotEmpty();
        }
    }
}
