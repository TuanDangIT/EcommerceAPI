using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class ProductNotFoundException : EcommerceException
    {
        public Guid Id { get; set; }
        public ProductNotFoundException(Guid id) : base($"Product: {id} was not found.")
        {
            Id = id;
        }
    }
}
