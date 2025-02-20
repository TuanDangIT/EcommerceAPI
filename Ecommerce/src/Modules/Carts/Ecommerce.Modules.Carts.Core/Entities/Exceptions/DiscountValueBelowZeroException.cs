using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    public class DiscountValueBelowZeroException : EcommerceException
    {
        public DiscountValueBelowZeroException() : base("Discount value must be higher or equal 0.")
        {
        }
    }
}
