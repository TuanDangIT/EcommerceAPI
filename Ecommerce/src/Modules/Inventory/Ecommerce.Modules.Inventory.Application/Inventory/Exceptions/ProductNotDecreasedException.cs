using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Exceptions
{
    internal class ProductNotDecreasedException : EcommerceException
    {
        public Guid Id { get; }
        public ProductNotDecreasedException(Guid id) : base($"Product: {id} quantity was not decreased.")
        {
            Id = id;
        }
    }
}
