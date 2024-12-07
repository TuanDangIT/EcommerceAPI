using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Stripe
{
    internal class StripeFailedRequestException : StripeException
    {
        public StripeFailedRequestException() : base("Failed to process Stripe request.")
        {

        }
    }
}
