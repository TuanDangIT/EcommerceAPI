using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Exceptions
{
    internal class DiscountAlreadyActivated(string code) : EcommerceException($"Code: {code} was already activated.")
    {
    }
}
