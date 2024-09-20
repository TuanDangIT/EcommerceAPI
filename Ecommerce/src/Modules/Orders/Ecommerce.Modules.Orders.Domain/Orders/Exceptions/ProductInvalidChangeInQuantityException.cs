using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal class ProductInvalidChangeInQuantityException : EcommerceException
    {
        public ProductInvalidChangeInQuantityException() : base("Cannot change quantity on product that doesn't have quantity.")
        {
        }
    }
}
