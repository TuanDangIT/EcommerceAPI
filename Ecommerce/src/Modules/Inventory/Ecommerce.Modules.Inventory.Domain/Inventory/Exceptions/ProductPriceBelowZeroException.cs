using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductPriceBelowZeroException : EcommerceException
    {
        public ProductPriceBelowZeroException() : base("Product's price must be higher or equal 0.")
        {
        }
    }
}
