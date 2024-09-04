using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductInvalidSkuException : EcommerceException
    {
        public ProductInvalidSkuException() : base("Product's SKU should be between 8 and 16 characters.")
        {
        }
    }
}
