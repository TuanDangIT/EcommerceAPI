using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Exception
{
    internal class ReturnProductNotFoundException : EcommerceException
    {
        public ReturnProductNotFoundException(int productId) : base($"Return product: {productId} was not found.")
        {
        }
        public ReturnProductNotFoundException(string sku) : base($"Return product: {sku} was not found.")
        {
            
        }
    }
}
