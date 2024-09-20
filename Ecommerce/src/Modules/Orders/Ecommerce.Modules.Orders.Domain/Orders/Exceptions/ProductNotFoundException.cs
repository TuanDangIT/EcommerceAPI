using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal sealed class ProductNotFoundException(string sku) : EcommerceException($"Product with SKU: {sku} was not found.")
    {
    }
}
