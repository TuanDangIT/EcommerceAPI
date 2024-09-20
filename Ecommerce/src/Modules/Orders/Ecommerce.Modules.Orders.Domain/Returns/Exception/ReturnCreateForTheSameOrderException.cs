using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Exception
{
    internal class ReturnCreateForTheSameOrderException(Guid orderId) : EcommerceException($"Cannot create a return for the same order: {orderId}")
    {
    }
}
