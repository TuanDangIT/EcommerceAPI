using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImagePathUrl { get; set; } = string.Empty;
    }
}
