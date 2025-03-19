using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Exception
{
    internal class ReturnProductNotFoundException(int productId, Guid returnId) : EcommerceException($"Return product: {productId} was not found for return: {returnId}.")
    {
    }
}
