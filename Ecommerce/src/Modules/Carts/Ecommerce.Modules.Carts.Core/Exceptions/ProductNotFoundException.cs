using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Ecommerce.Modules.Carts.Tests.Unit")]
namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class ProductNotFoundException : EcommerceException
    {
        public ProductNotFoundException(Guid productId) : base($"Product with ID: {productId} was not found.")
        {
            
        }
        public ProductNotFoundException(string sku) : base($"Product with SKU: {sku} was not found.")
        {
            
        }
    }
}
