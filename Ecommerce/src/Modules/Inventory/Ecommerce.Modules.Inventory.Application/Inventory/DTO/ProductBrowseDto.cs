using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.DTO
{
    public class ProductBrowseDto
    {
        public Guid Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public int? Reserved { get; set; }
        public decimal Price { get; set; }
        public bool IsListed { get; set; }
    }
}
