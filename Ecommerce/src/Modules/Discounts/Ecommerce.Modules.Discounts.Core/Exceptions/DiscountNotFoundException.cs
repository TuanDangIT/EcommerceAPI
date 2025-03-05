using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Exceptions
{
    internal class DiscountNotFoundException : EcommerceException
    {
        public DiscountNotFoundException(string code) : base($"Code: {code} was not found.")
        {
            
        }
        public DiscountNotFoundException(int discountId) : base($"Code: {discountId} was not found.")
        {
            
        }
    }
}
