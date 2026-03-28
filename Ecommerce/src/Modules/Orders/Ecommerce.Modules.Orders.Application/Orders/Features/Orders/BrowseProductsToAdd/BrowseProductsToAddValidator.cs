using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.BrowseProductsToAdd
{
    internal class BrowseProductsToAddValidator : AbstractValidator<BrowseProductsToAdd>
    {
        public BrowseProductsToAddValidator()
        {
            RuleFor(b => b.OrderId)
                .NotEmpty()
                .NotNull();
            RuleFor(b => b.SearchTerm)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100);
        }
    }
}
