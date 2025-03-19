using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Exception
{
    internal class ReturnInvalidSetQuantityException : EcommerceException
    {
        public ReturnInvalidSetQuantityException() : base("Set quantity must be higher or equal 0.")
        {
        }
    }
}
