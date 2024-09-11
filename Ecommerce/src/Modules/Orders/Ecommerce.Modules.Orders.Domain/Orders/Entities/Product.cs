using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public string SKU { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string ImagePathUrl { get; private set; } = string.Empty;
        public Order Order { get; private set; } = new();
        public Guid OrderId { get; private set; }
        public Product(string name, decimal price, string imagePathUrl)
        {
            Name = name;
            Price = price;
            ImagePathUrl = imagePathUrl;
        }
        public Product()
        {

        }
    }
}
