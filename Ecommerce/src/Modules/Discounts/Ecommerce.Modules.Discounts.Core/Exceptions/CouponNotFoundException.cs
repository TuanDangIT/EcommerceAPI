using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Exceptions
{
    public class CouponNotFoundException(string stripeCouponId) : EcommerceException($"Coupon with an stripe ID: {stripeCouponId} was not found.")
    {
    }
}
