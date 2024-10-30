using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Shipping.Features.BrowseShippings
{
    internal class BrowseShipmentsValidator : AbstractValidator<BrowseShipments>
    {
        private readonly string[] _availableFilters = ["Id", "TrackingNumber", "LabelCreatedAt", "Service", "Receiver.FirstName", "Receiver.LastName",
            "Receiver.Phone", "Receiver.Email", ];
        public BrowseShipmentsValidator()
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
