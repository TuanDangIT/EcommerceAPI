using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class CartProductDiscountedPriceBelowZeroException : EcommerceException
    {
        public CartProductDiscountedPriceBelowZeroException() : base("Cart product's discounted price must be higher than 0.")
        {
        }
    }
}
