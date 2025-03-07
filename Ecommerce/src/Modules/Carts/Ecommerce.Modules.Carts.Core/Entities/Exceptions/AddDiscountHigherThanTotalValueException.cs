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
        public AddDiscountHigherThanTotalValueException() : base("Cannot add discount that is higher than total value of a cart.")
        {
        }
    }
}
