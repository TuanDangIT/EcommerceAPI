using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Exception
{
    internal class ReturnProductQuantityExceedLimitException(int quantity, int limit) : 
        EcommerceException($"Cannot set the quantity: {quantity} for the return product, because it exceeds the total amount: {limit} for order.")
    {
    }
}
