using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    internal class OrderCannotReturnException : EcommerceException
    {
        public OrderCannotReturnException(string message) : base(message)
        {
        }
    }
}
