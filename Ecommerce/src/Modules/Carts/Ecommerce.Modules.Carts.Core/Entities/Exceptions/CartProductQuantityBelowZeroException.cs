using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class CartProductQuantityBelowZeroException : EcommerceException
    {
        public CartProductQuantityBelowZeroException() : base("Quantity can't be below 0.")
        {
        }
    }
}
