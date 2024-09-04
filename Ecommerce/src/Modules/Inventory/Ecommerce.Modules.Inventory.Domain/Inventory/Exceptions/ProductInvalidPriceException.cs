using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductInvalidPriceException : EcommerceException
    {
        public ProductInvalidPriceException() : base("Product's price should be greater than or equal 0.")
        {
        }
    }
}
