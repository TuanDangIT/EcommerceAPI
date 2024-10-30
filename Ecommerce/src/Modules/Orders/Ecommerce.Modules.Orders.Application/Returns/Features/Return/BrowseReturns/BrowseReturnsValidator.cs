using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.BrowseReturns
{
    internal class BrowseReturnsValidator : AbstractValidator<BrowseReturns>
    {
        private readonly string[] _availableFilters = ["Id", "OrderId", "ReasonForReturn", "AdditionalNote", "IsFullReturn", "CreatedAt",
            "Order.TotalSum", "Order.Payment", "Order.Status"];
        public BrowseReturnsValidator()
        {
            RuleForEach(b => b.Filters)
                .Custom((keyValuePair, context) =>
                {
                    if (!_availableFilters.Contains(keyValuePair.Key))
                    {
                        context.AddFailure($"Provided filter is not supported. Please use the following ones: {string.Join(", ", _availableFilters)}.");
                    }
                });
        }
    }
}
