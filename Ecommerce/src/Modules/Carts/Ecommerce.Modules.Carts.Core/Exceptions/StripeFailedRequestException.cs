using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class StripeFailedRequestException : StripeException
    {
        public StripeFailedRequestException() : base("Failed to process Stripe request.")
        {
            
        }
        public StripeFailedRequestException(string stripeMessage) : base($"Failed to process Stripe request. Message: {stripeMessage}")
        {
            
        }
    }
}
