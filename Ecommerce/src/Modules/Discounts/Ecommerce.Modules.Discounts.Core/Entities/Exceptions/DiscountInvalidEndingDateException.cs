using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities.Exceptions
{
    internal class DiscountInvalidEndingDateException(DateTime endingDate) : EcommerceException($"Ending date: {endingDate} is invalid. Cannot be in the past.")
    {
    }
}
