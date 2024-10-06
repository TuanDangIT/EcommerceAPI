using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public abstract class Coupon
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; } = string.Empty;
        public CouponType Type { get; protected set; }
        public string StripeCouponId { get; protected set; } = string.Empty;
        public int Redemptions => _discounts.Sum(d => d.Redemptions);
        public int TotalSumOfDiscounts => _discounts.Count();
        public DateTime? UpdatedAt { get; protected set; } 
        public DateTime CreatedAt { get; protected set; }
        protected readonly List<Discount> _discounts = [];
        public IEnumerable<Discount> Discounts => _discounts;
        public Coupon(string name, DateTime createdAt)
        {
            Name = name;
            CreatedAt = createdAt;
        }
        public Coupon(string name, DateTime createdAt, string stripeCouponId)
        {
            Name = name;
            CreatedAt = createdAt;
            StripeCouponId = stripeCouponId;
        }
        public Coupon()
        {
            
        }
        public void AddDiscount(Discount discount, DateTime updatedAt)
        {
            _discounts.Add(discount);
            UpdatedAt = updatedAt;
        }
        public void ChangeName(string name, DateTime updatedAt)
        {
            Name = name;
            UpdatedAt = updatedAt;
        }
    }
}
