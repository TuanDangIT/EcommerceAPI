using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class ProductsNotDeletedException : EcommerceException
    {
        public ProductsNotDeletedException() : base("One or more products were not deleted")
        {
        }
    }
}
