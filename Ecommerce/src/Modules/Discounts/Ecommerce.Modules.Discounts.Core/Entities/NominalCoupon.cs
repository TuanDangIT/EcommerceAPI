using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public class NominalCoupon : Coupon
    {
        public decimal NominalValue { get; set; }
        public NominalCoupon(string name, decimal nominalValue, DateTime createdAt) : base(name, createdAt)
        {
            NominalValue = nominalValue; 
        }
        public NominalCoupon(string code, decimal nominalValue, DateTime createdAt, string stripeCouponId) : base(code, createdAt, stripeCouponId)
        {
            NominalValue = nominalValue;
        }
    }
}
