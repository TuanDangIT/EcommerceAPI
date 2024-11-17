using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.BrowseInvoices
{
    internal class BrowseInvoicesValidator : AbstractValidator<BrowseInvoices>
    {
        private readonly string[] _availableFilters = ["Id", "InvoiceNo", "CreatedAt", "OrderId", "Order.TotalSum", "Order.Status", "Order.Payment"];
        public BrowseInvoicesValidator()
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
