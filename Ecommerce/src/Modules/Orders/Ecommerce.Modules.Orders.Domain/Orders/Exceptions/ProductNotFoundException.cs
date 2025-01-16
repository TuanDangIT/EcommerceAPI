using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal sealed class ProductNotFoundException : EcommerceException
    {
        public ProductNotFoundException(int productId) : base($"Product: {productId} was not found.")
        {
        }
        public ProductNotFoundException(string sku) : base($"Product: {sku} was not found.")
        {
        }
    }
}
