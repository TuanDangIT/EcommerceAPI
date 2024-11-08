using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities.Exceptions
{
    internal class CouponNominalValueBelowOrEqualZeroException : EcommerceException
    {
        public CouponNominalValueBelowOrEqualZeroException() : base("Nominal value should be greater than 0.")
        {

        }
    }
}
