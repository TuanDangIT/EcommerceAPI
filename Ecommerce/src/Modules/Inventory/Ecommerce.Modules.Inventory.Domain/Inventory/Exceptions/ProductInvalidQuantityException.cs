using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductInvalidQuantityException : EcommerceException
    {
        public ProductInvalidQuantityException() : base("Product's quantity should be greater than or equal 0.")
        {
        }
    }
}
