using Ecommerce.Modules.Products.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.DTO
{
    public class ProductBrowseDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double AverageGrade { get; set; }
    }
}
