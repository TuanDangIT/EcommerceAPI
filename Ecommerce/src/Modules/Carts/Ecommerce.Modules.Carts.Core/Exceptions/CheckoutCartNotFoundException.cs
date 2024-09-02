using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class CheckoutCartNotFoundException : EcommerceException
    {
        public Guid Id { get; set; }
        public CheckoutCartNotFoundException(Guid id) : base($"Checkout cart: {id} was not found.")
        {
            Id = id;
        }
    }
}
