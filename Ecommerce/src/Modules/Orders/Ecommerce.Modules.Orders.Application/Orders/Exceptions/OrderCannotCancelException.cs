using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    internal class OrderCannotCancelException : EcommerceException
    {
        public OrderCannotCancelException() : base("Cannot cancel an order after 30 minutes of placing it.")
        {
        }
    }
}
