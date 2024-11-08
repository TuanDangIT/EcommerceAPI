using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Ecommerce.Modules.Discounts.Core.Entities.Exceptions;
using Ecommerce.Shared.Abstractions.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public class Offer : BaseEntity<int>, IAuditable
    {
        public decimal OfferedPrice { get; private set; }
        public decimal OldPrice { get; private set; }
        public decimal Difference => OldPrice - OfferedPrice;
        public string Reason { get; private set; } = string.Empty;
        public string SKU { get; private set; } = string.Empty;
        public string ProductName { get; private set; } = string.Empty;
        public OfferStatus Status { get; private set; } = OfferStatus.Initialized;
        public DateTime? ExpiresAt { get; private set; } 
        public string? Code { get; private set; }
        public Guid CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Offer(string sku, string productName, decimal offeredPrice, decimal oldPrice, string reason, Guid customerId)
        {
            SKU = sku;
            ProductName = productName;
            OfferedPrice = offeredPrice;
            OldPrice = oldPrice;
            Reason = reason;
            CustomerId = customerId;
        }
        public Offer()
        {
            
        }
        public void Accept(DateTime expiresAt)
        {
            if(Status is OfferStatus.Accepted)
            {
                throw new OfferCannotAcceptException(Id);
            }
            Status = OfferStatus.Accepted;
            SetExpiresTime(expiresAt);
        }
        public void Reject()
            => Status = OfferStatus.Rejected;
        public void SetCode(string code)
            => Code = code;
        public void SetExpiresTime(DateTime expiresAt)
        {
            if(expiresAt < TimeProvider.System.GetUtcNow().UtcDateTime)
            {
                throw new OfferInvalidExpiresAtException(expiresAt);
            }
            ExpiresAt = expiresAt;
        }
    }
}
