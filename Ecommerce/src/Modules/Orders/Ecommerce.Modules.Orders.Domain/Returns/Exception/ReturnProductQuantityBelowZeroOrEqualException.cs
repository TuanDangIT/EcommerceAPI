using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Exception
{
    internal class ReturnProductQuantityBelowZeroOrEqualException : EcommerceException
    {
        public ReturnProductQuantityBelowZeroOrEqualException() : base("Return product's quantity must be higher than 0.")
        {
        }
    }
}
