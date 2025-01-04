using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class CheckoutCartCannotRemoveDiscountException : EcommerceException
    {
        public CheckoutCartCannotRemoveDiscountException() : base("Cannot remove discount if it doesn't exist.")
        {
        }
    }
}
