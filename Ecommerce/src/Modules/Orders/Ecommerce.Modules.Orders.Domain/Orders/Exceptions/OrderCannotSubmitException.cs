using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal class OrderCannotSubmitException : EcommerceException
    {
        public OrderCannotSubmitException() : base("Cannot submit an order more than twice.")
        {
        }
    }
}
