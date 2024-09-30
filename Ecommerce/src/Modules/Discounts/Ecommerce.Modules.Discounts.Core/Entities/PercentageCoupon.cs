using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public class PercentageCoupon : Coupon
    {
        public decimal Percent { get; set; }
        public PercentageCoupon(string name, decimal percent, DateTime createdAt) : base(name, createdAt)
        {
            Percent = percent;
        }
        public PercentageCoupon(string code, decimal percent, DateTime createdAt, string stripeCouponId) : base(code, createdAt, stripeCouponId)
        {
            Percent = percent;
        }
    }
}
