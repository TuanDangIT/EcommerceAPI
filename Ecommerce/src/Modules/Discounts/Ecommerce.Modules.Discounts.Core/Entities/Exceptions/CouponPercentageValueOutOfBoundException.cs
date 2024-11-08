using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities.Exceptions
{
    internal class CouponPercentageValueOutOfBoundException : EcommerceException
    {
        public CouponPercentageValueOutOfBoundException() : base("Percentage value should be greaten than 0 and less or equal 100.")
        {
        }
    }
}
