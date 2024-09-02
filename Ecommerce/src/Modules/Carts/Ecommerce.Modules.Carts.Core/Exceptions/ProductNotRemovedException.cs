using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class ProductNotRemovedException : EcommerceException
    {
        public ProductNotRemovedException() : base("Product was not removed.")
        {
        }
    }
}
