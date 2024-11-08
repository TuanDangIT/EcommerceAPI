using Ecommerce.Modules.Discounts.Core.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public class PercentageCoupon : Coupon
    {
        public decimal Percent { get; private set; }
        public PercentageCoupon() : base()
        {

        }
        public PercentageCoupon(string code, decimal percent, string stripeCouponId) : base(code, stripeCouponId)
            => Percent = percent > 0 && percent <= 100 ? percent : throw new CouponPercentageValueOutOfBoundException();
    }
}
