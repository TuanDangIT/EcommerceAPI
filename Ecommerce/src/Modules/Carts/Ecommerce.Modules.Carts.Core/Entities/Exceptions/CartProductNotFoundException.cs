using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class CartProductNotFoundException : EcommerceException
    {
        public CartProductNotFoundException(Guid productId) : base($"Product: {productId} was not found.")
        {
        }
        public CartProductNotFoundException(Guid productId, Guid cartId) : base($"Product: {productId} was not found in cart: {cartId}.")
        {
        }
    }
}
