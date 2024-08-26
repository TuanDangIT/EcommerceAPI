using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string SKU { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int? Quantity { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public string? AdditionalDescription { get; private set; }
        public List<Parameter>? Parameters { get; set; } = new();
        //private readonly List<Parameter> _parameters = new();
        //public IEnumerable<Parameter> Parameters => _parameters;
        //private List<ProductParameter> _productParameters = new();
        //public IEnumerable<ProductParameter> ProductParameters => _productParameters;
        public string? Manufacturer { get; set; } 
        //public List<Image> _images = new();
        //public IEnumerable<Image> Images => _images;
        public List<string> ImageUrls { get; set; } = new();
        public string Category { get; set; } = string.Empty;
        public List<Review>? Reviews { get; set; } = new();
        //public DateTime CreatedAt { get; private set; }
        //public DateTime? UpdatedAt { get; private set; }
    }
}
