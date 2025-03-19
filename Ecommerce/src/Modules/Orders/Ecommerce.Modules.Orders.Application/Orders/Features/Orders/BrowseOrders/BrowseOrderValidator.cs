using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders
{
    public class BrowseOrderValidator : AbstractValidator<BrowseOrders>
    {
        private readonly string[] _availableFilters = ["Id", "TotalSum", "CreatedAt", "Discount.Code", "Discount.Type", "Payment", "Status", 
            "Customer.UserId", "Customer.FirstName", "Customer.LastName", "Customer.Email", "Customer.FirstName",
            "Shipments.TrackingNumber", "Shipments.Service", "Shipments.Id", "Shipments.LabelCreatedAt", "Products.SKU", "Products.Name"];
        public BrowseOrderValidator()
        {
            RuleForEach(b => b.Filters)
                .Custom((keyValuePair, context) =>
                {
                    if (!_availableFilters.Select(a => a.ToLower()).Contains(keyValuePair.Key.ToLower()))
                    {
                        context.AddFailure($"Provided filter is not supported. Please use the following ones: {string.Join(", ", _availableFilters)}.");
                    }
                });
        }
    }
}
