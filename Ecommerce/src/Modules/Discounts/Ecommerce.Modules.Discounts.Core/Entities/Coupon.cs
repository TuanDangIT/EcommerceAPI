using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public abstract class Coupon : BaseEntity<int>, IAuditable
    {
        public string Name { get; protected set; } = string.Empty;
        public CouponType Type { get; protected set; }
        public string StripeCouponId { get; protected set; } = string.Empty;
        public int Redemptions => _discounts.Sum(d => d.Redemptions);
        protected readonly List<Discount> _discounts = [];
        public IEnumerable<Discount> Discounts => _discounts;
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public bool HasDiscounts => _discounts.Count != 0;
        public Coupon(string name, string stripeCouponId)
        {
            Name = name;
            StripeCouponId = stripeCouponId;
        }
        protected Coupon()
        {
            
        }
        public void AddDiscount(Discount discount)
            => _discounts.Add(discount);
        public void ChangeName(string name)
            => Name = name;
    }
}
