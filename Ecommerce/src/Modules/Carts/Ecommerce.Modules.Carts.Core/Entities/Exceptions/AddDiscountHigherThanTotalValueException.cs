using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class AddDiscountHigherThanTotalValueException : EcommerceException
    {
        public AddDiscountHigherThanTotalValueException(int discountId, decimal totalValue) : base($"Cannot add discount: {discountId} that is higher than total value: {totalValue} of a cart.")
        {
        }
    }
}
