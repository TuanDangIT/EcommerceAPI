using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Exception
{
    internal class ReturnProductUnitPriceBelowZeroException : EcommerceException
    {
        public ReturnProductUnitPriceBelowZeroException() : base("Return product's unit price must be higher or equal 0.")
        {
        }
    }
}
