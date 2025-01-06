using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal class OrderShippingPriceBelowZeroException : EcommerceException
    {
        public OrderShippingPriceBelowZeroException() : base("Order's shipping price must be higher or equal 0.")
        {
        }
    }
}
