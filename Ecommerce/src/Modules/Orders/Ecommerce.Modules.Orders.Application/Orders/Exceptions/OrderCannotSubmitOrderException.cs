using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    internal class OrderCannotSubmitOrderException : EcommerceException
    {
        public OrderCannotSubmitOrderException()
           : base($"Cannot submit order because it is not a draft.")
        {
        }
    }
}
