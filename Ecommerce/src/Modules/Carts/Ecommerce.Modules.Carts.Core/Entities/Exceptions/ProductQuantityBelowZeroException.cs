using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class ProductQuantityBelowZeroException : EcommerceException
    {
        public ProductQuantityBelowZeroException() : base("Prooduct's quantity must be higher or equal 0.")
        {
        }
    }
}
