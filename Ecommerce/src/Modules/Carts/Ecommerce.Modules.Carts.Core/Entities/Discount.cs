using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class Discount : BaseEntity<int>
    {
        public string Code { get; private set; } = string.Empty;
        public DiscountType Type { get; private set; }
        public string? SKU { get; private set; }
        public Guid? CustomerId { get; private set; }
        public string? StripePromotionCodeId { get; private set; } 
        public decimal Value { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        private readonly List<CheckoutCart> _checkoutCarts = [];
        public IEnumerable<CheckoutCart> CheckoutCarts => _checkoutCarts;
        private readonly List<Cart> _carts = [];
        public IEnumerable<Cart> Carts => _carts;
        public bool HasCustomerId => CustomerId is not null;
        public Discount(string code, DiscountType type, decimal value, DateTime? expiresAt, string stripePromotionCodeId)
        {
            Code = code;
            Type = type;
            Value = value;
            StripePromotionCodeId = stripePromotionCodeId;
            if (expiresAt is not null && expiresAt < TimeProvider.System.GetUtcNow().UtcDateTime)
            {
                throw new DiscountInvalidExpiresAtDateException((DateTime)expiresAt!);
            }
            ExpiresAt = expiresAt;
        }
        public Discount(string code, string sku, decimal value, Guid customerId, DateTime expiresAt)
        {
            if(value >= 0)
            {
                throw new PropertyValueBelowOrEqualZeroException(nameof(Discount));
            }
            Code = code;
            Type = DiscountType.NominalDiscount;
            SKU = sku;
            Value = value;
            CustomerId = customerId;
            if (expiresAt < TimeProvider.System.GetUtcNow().UtcDateTime)
            {
                throw new DiscountInvalidExpiresAtDateException(expiresAt!);
            }
            ExpiresAt = expiresAt;
        }
        private Discount()
        {
            
        }
    }
}
