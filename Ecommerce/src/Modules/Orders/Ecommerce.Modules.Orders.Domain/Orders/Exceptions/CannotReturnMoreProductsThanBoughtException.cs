using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal class CannotReturnMoreProductsThanBoughtException(int productId, int boughtQuantity, int returnQuantity) : 
        EcommerceException($"Cannot return {returnQuantity} units of product: {productId} as only {boughtQuantity} were purchased.")
    {
    }
}
