using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class DiscountRequiredCartTotalValueBelowZeroException : EcommerceException
    {
        public DiscountRequiredCartTotalValueBelowZeroException() : base("Required total value to use discount must be higher than 0.")
        {
        }
    }
}
