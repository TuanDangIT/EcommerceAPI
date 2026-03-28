using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    public class ProductNotFoundException : EcommerceException
    {
        public ProductNotFoundException(Guid productId) : base($"Product with ID: {productId} was not found.")
        {
            
        }

        public ProductNotFoundException(string sku) : base($"Product with SKU: {sku} was not found.")
        {
            
        }
    }
}
