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
        public List<AuctionParameter>? Parameters { get; set; } 
        public string? Manufacturer { get; set; }
        public List<string> ImagePathUrls { get; set; } = [];
        public string Category { get; set; } = string.Empty;
        private readonly List<Review> _review = [];
        public IEnumerable<Review> Reviews => _review;
        public DateTime CreatedAt { get; private set; }
        public Auction(Guid id, string sku, string name, decimal price, string description, List<string> imagePathUrls, string category, DateTime createdAt, 
            int? quantity = null, string? additionalDescription = null, List<AuctionParameter>? parameters = null, string? manufacturer = null)
        {
            Id = id;
            SKU = sku;
            Name = name;
            Price = price;
            Description = description;
            ImagePathUrls = imagePathUrls;
            Category = category;
            CreatedAt = createdAt;
            Quantity = quantity;
            AdditionalDescription = additionalDescription;
            Parameters = parameters;
            Manufacturer = manufacturer;
        }
        public Auction()
        {
            
        }
        public void AddReview(Review review)
            => _review.Add(review);
    }
}
