using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    public class OrderDraftException(Guid orderId) : EcommerceException($"This operation cannot be completed because order: {orderId} is still in draft status.")
    {
    }
}
