using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductInvalidChangeOfQuantityException : EcommerceException
    {
        public ProductInvalidChangeOfQuantityException() : base("Cannot change quantity on product that doesn't have quantity.")
        {
        }
    }
}
