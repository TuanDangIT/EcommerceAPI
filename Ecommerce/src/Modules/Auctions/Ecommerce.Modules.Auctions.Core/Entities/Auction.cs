using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.Entities
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
        public List<Parameter>? Parameters { get; set; } = [];
        public string? Manufacturer { get; set; } 
        public List<string> ImagePathUrls { get; set; } = [];
        public string Category { get; set; } = string.Empty;
        public List<Review> Reviews { get; set; } = [];
        public void AddReview(Review review)
            => Reviews.Add(review);
    }
}
