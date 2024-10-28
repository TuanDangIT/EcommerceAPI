using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders
{
    internal class BrowseOrderValidator : AbstractValidator<BrowseOrders>
    {
        private readonly string[] _availableFilters = ["Id", "TotalSum", "OrderPlacedAt", "DiscountCode", "Payment", "Status", 
            "Customer.UserId", "Customer.FirstName", "Customer.LastName", "Customer.Email", "Customer.FirstName",
            "Shipment.TrackingNumber", "Shipment.Service", "Shipment.Id", "Shipment.LabelCreatedAt"];
        public BrowseOrderValidator()
        {
            RuleForEach(b => b.Filters)
                .Custom((keyValuePair, context) =>
                {
                    if (!_availableFilters.Contains(keyValuePair.Key))
                    {
                        context.AddFailure($"Provided filter is not supported. Please use the following ones: {string.Join(", ", _availableFilters)}");
                    }
                });
        }
    }
}
