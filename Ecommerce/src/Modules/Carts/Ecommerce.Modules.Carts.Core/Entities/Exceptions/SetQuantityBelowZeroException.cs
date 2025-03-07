using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class SetQuantityBelowZeroException : EcommerceException
    {
        public SetQuantityBelowZeroException() : base("Quantity for set must be higher than 0.")
        {
        }
    }
}
