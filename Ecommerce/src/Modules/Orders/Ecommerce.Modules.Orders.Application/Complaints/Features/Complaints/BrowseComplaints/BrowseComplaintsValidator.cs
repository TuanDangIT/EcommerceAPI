using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.BrowseComplaints
{
    internal class BrowseComplaintsValidator : AbstractValidator<BrowseComplaints>
    {
        private readonly string[] _availableFilters = ["Id", "Title", "OrderId", "Title", "Description", "AdditionalNote", "Decision.DecisionText",
            "Status", "CreatedAt"];
        public BrowseComplaintsValidator()
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
