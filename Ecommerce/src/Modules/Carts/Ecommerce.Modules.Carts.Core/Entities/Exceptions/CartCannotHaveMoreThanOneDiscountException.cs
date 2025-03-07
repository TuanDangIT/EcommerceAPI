using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class CartCannotHaveMoreThanOneDiscountException(Guid cartId) : EcommerceException($"Cart: {cartId} cannot have more than one discount code applied.")
    {
    }
}
