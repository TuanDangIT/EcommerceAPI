using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.DTO
{
    public class ProductDetailsDto
    {
        public Guid Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string? EAN { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int VAT { get; set; }
        public int? Quantity { get; set; }
        public int? Reserved { get; set; }
        public string? Location { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsListed { get; set; }
        public string? AdditionalDescription { get; set; }
        public string? Manufacturer { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public List<ProductDetailsImageDto> Images { get; set; } = [];
        public List<ProductDetailsParameterDto> Parameters { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class ProductDetailsImageDto
    {
        public string ImageUrlPath { get; set; } = string.Empty;
        public int Order { get; set; }
    }
    public class ProductDetailsParameterDto
    {
        public string Parameter { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
