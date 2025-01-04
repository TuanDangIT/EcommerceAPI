using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class CheckoutCartCannotApplyIndividualDiscountException(string sku) : EcommerceException($"Cannot apply this discount for a product: {sku} that does not exist in checkout cart.")
    {
    }
}
