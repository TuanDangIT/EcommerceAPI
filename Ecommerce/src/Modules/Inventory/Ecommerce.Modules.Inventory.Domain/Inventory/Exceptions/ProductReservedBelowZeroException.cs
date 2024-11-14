using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductReservedBelowZeroException : EcommerceException
    {
        public ProductReservedBelowZeroException() : base("Reserved products must be equal or higher than 0.")
        {
        }
    }
}
