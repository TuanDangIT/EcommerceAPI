using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.UnlistProducts
{
    internal sealed class UnlistProductValidator : AbstractValidator<UnlistProducts>
    {
        public UnlistProductValidator()
        {
            RuleFor(up => up.ProductIds)
                .NotNull()
                .NotEmpty();
        }
    }
}
