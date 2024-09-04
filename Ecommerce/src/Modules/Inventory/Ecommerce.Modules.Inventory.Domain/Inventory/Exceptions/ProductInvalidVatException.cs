using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductInvalidVatException : EcommerceException
    {
        public ProductInvalidVatException() : base("Product's VAT should be greater than or equal 0% and less than or equal 100%.")
        {
        }
    }
}
