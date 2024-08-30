using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Exceptions
{
    internal class ProductNotAllFoundException : EcommerceException
    {
        public IEnumerable<Guid> ProductIds { get; }
        public ProductNotAllFoundException(IEnumerable<Guid> productIds) : base($"Products: {string.Join(", ", productIds)} were not found")
        {
            ProductIds = productIds;
        }
    }
}
