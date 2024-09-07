using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string ImagePathUrl { get; private set; } = string.Empty;
        private List<CartProduct> _cartProducts = [];
        public IEnumerable<CartProduct> CartProducts => _cartProducts;
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
