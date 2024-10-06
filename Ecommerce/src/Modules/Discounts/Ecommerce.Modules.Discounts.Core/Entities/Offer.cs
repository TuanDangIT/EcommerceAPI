using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public class Offer
    {
        public int Id { get; set; } 
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Difference => OldPrice - Price;
        public string Reason { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public OfferStatus Status { get; set; } = OfferStatus.Initialized;
        public Guid CustomerId { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Offer(string sku, decimal price, decimal oldPrice, string reason, Guid customerId, DateTime createdAt)
        {
            SKU = sku;
            Price = price;
            OldPrice = oldPrice;
            Reason = reason;
            CustomerId = customerId;
            CreatedAt = createdAt;
        }
        public Offer()
        {
            
        }
        public void Accept(DateTime updatedAt)
        {
            Status = OfferStatus.Accepted;
            UpdatedAt = updatedAt;
        }
        public void Reject(DateTime updatedAt)
        {
            Status = OfferStatus.Rejected;
            UpdatedAt = updatedAt;
        }
    }
}
