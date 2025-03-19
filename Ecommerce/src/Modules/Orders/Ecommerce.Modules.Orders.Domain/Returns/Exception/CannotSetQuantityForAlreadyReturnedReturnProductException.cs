using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Exception
{
    internal class CannotSetQuantityForAlreadyReturnedReturnProductException(int productId) : EcommerceException($"Cannot set quantity for return product: {productId} that is in returned status.")
    {
    }
}
