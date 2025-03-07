using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class ProductOutOfStockException : EcommerceException
    {
        public ProductOutOfStockException(Guid productId) : base($"Product: {productId} is out of stock.")
        {
        }
    }
}
