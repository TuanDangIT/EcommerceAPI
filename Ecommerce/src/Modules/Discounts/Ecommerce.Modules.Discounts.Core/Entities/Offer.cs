using Ecommerce.Modules.Discounts.Core.Entities.Enums;
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
        public string Justification { get; set; } = string.Empty;
        public OfferStatus Status { get; set; } = OfferStatus.Initialized;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Offer(decimal price, string justification, DateTime createdAt)
        {
            Price = price;
            Justification = justification;
            CreatedAt = createdAt;
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
