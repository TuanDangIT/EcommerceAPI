using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.DeleteProduct
{
    internal class DeleteProductValidator : AbstractValidator<DeleteProduct>
    {
        public DeleteProductValidator()
        {
            RuleFor(d => d.ProductId)
                .NotNull()
                .NotEmpty();
        }
    }
}
