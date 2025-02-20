using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    public class CheckoutCartCalculatedTotalBelowOrEqualZeroException : EcommerceException
    {
        public CheckoutCartCalculatedTotalBelowOrEqualZeroException() : base("Checkout cart's total sum must be higher than 0.")
        {
        }
    }
}
