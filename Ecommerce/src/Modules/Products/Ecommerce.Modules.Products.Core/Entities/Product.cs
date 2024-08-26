using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? AdditionalDescription { get; set; }
        public List<Parameter>? Parameters { get; set; } = new();
        //private readonly List<Parameter> _parameters = new();
        //public IEnumerable<Parameter> Parameters => _parameters;
        //private List<ProductParameter> _productParameters = new();
        //public IEnumerable<ProductParameter> ProductParameters => _productParameters;
        public string? Manufacturer { get; set; } 
        //public List<Image> _images = new();
        //public IEnumerable<Image> Images => _images;
        public List<string> ImagePathUrls { get; set; } = new();
        public string Category { get; set; } = string.Empty;
        public List<Review> Reviews { get; set; } = new();
        //public DateTime CreatedAt { get; private set; }
        //public DateTime? UpdatedAt { get; private set; }
        public void AddReview(Review review)
            => Reviews.Add(review);
    }
}
