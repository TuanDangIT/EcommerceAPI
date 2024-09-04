using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductInvalidLocationException : EcommerceException
    {
        public ProductInvalidLocationException() : base("Product's location should be 64 characters.")
        {
        }
    }
}
