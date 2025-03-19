using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    internal class OrderCannotReturnRejectedReturnException(Guid orderId) : EcommerceException($"Cannot process a return for order: {orderId} that has already been rejected and returned")
    {
    }
}
