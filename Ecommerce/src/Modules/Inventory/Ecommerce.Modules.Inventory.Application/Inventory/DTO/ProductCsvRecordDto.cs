using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.DTO
{
    public class ProductCsvRecordDto
    {
        public string SKU { get; set; } = string.Empty;
        public string? EAN { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int VAT { get; set; }
        public int? Quantity { get; set; }
        public string? Location { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? AdditionalDescription { get; set; }
        public string? Manufacturer { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public Dictionary<string, string> Parameters { get; set; } = [];
        public List<string> Images { get; set; } = [];
    }
}
