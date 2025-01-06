using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal class ProductQuantityBelowOrEqualZeroException : EcommerceException
    {
        public ProductQuantityBelowOrEqualZeroException() : base("Product's quantity must be higher than 0.")
        {
        }
    }
}
