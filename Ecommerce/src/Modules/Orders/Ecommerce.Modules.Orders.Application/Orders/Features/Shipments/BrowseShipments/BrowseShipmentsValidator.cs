using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.BrowseShipments
{
    internal class BrowseShipmentsValidator : AbstractValidator<BrowseShipments>
    {
        private readonly string[] _availableFilters = ["Id",
            "TrackingNumber",
            "LabelCreatedAt",
            "Service",
            "Receiver.FirstName",
            "Receiver.LastName",
            "Receiver.Phone",
            "Receiver.Email",
        ];
        public BrowseShipmentsValidator()
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
