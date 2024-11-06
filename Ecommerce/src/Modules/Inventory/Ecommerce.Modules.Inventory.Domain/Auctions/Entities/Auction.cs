using Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Entities
{
    public class Auction
    {
        public Guid Id { get; private set; }
        public string SKU { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int? Quantity { get; private set; }
        public bool HasQuantity => Quantity != null;
        public string Description { get; private set; } = string.Empty;
        public string? AdditionalDescription { get; private set; }
        public List<AuctionParameter>? Parameters { get; private set; } 
        public string? Manufacturer { get; private set; }
        public List<string> ImagePathUrls { get; private set; } = [];
        public string Category { get; private set; } = string.Empty;
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
        public void DecreaseQuantity(int quantity)
        {
            if(Quantity is null)
            {
                throw new AuctionInvalidChangeOfQuantityException();
            }
            if (Quantity < quantity)
            {
                throw new AuctionQuantityBelowZeroException();
            }
            Quantity -= quantity;
        }
        public void IncreaseQuantity(int quantity)
        { 
            if(Quantity is null)
            {
                throw new AuctionInvalidChangeOfQuantityException();
            }
            Quantity += quantity;
        }
    }
}
