using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    internal class OrderCannotReturnRejectedReturnException : EcommerceException
    {
        public OrderCannotReturnRejectedReturnException() : base("Cannot return for order which has been already returned, but rejected.")
        {
        }
    }
}
