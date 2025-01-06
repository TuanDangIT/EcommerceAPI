using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductVatBelowZeroException : EcommerceException
    {
        public ProductVatBelowZeroException() : base("Product's VAT must be higher than 0.")
        {
        }
    }
}
