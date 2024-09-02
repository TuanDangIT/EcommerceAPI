using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class CartNotFoundException : EcommerceException
    {
        public Guid Id { get; set; }
        public CartNotFoundException(Guid id) : base($"Cart: {id} was not found.")
        {
            Id = id;
        }
    }
}
