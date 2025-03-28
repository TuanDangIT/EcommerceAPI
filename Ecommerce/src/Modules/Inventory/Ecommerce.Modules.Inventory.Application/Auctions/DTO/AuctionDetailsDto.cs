using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.DTO
{
    public class AuctionDetailsDto
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
        public List<string> ImageUrls { get; set; } = [];
        public string? Category { get; set; } = string.Empty;
        public double AverageGrade { get; set; }
        public List<ReviewDto>? Reviews { get; set; } = [];
    }
}
