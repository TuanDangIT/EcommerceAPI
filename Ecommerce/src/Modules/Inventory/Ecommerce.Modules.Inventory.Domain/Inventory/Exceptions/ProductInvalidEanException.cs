using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductInvalidEanException : EcommerceException
    {
        public ProductInvalidEanException() : base("Product's EAN should be 13 characters.")
        {
        }
    }
}
