using Ecommerce.Modules.Products.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.DTO
{
    public class ProductDetailsDto
    {
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? AdditionalDescription { get; set; }
        public List<Parameter>? Parameters { get; set; } = [];
        public string? Manufacturer { get; set; }
        public List<string> ImageUrls { get; set; } = [];
        public string Category { get; set; } = string.Empty;
        public double AverageGrade { get; set; }
        public List<Review>? Reviews { get; set; } = [];
    }
}
