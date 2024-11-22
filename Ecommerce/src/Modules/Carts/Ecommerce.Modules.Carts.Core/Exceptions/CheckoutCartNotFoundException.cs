using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class CheckoutCartNotFoundException : EcommerceException
    {
        public CheckoutCartNotFoundException(Guid checkoutCartId) : base($"Checkout cart: {checkoutCartId} was not found.")
        {

        }
        public CheckoutCartNotFoundException(string sessionId) : base($"Checkout cart with session id: {sessionId} was not found")
        {
            
        }
    }
}
