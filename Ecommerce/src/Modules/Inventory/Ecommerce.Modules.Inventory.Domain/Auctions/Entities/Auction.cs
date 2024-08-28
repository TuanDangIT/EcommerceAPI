using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Entities
{
    public class Auction
    {
        public Guid Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? AdditionalDescription { get; set; }
        public List<AuctionParameter>? Parameters { get; set; } = [];
        public string? Manufacturer { get; set; }
        public List<string> ImagePathUrls { get; set; } = [];
        public string Category { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        private readonly List<Review> _review = [];
        public IEnumerable<Review> Reviews => _review;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public void AddReview(Review review)
            => _review.Add(review);
    }
}
