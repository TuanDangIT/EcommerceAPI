using Ecommerce.Modules.Discounts.Core.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public class NominalCoupon : Coupon
    {
        public decimal NominalValue { get; private set; }
        private NominalCoupon() : base()
        {

        }
        public NominalCoupon(string code, decimal nominalValue, string stripeCouponId) : base(code, stripeCouponId)
            => NominalValue = nominalValue > 0 ? nominalValue : throw new CouponNominalValueBelowOrEqualZeroException();
    }
}
