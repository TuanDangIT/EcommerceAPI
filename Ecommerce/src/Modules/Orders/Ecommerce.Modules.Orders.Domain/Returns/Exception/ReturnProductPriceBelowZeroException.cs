using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Exception
{
    internal class ReturnProductPriceBelowZeroException : EcommerceException
    {
        public ReturnProductPriceBelowZeroException() : base("Return product's price must be higher or equal 0.")
        {
        }
    }
}
