using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string? EAN { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int VAT { get; set; }
        public int? Quantity { get; set; }
        public string? Location { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? AdditionalDescription { get; set; }
        public List<Parameter> Parameters { get; set; } = new();
        //private readonly List<Parameter> _parameters = new List<Parameter>();
        //public IEnumerable<Parameter> Parameters => _parameters;
        public Manufacturer Manufacturer { get; set; } = new();
        public Guid ManufacturerId { get; set; }
        public List<Image> Images { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
