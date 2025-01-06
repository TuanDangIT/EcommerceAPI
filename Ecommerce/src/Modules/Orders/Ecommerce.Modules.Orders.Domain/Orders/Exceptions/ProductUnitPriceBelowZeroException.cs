using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal class ProductUnitPriceBelowZeroException : EcommerceException
    {
        public ProductUnitPriceBelowZeroException() : base("Product's unit price must be higher or equal 0.")
        {
        }
    }
}
