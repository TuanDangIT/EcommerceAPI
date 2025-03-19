using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Exception
{
    internal class CannotRemoveAlreadyReturnedReturnProductException(int productId) : EcommerceException($"Cannot delete return product with Id: {productId} that status is returned.")
    {
    }
}
