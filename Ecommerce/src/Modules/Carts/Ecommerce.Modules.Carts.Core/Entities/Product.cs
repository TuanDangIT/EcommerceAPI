using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class Product
    {
        [JsonInclude]
        public Guid Id { get; private set; }
        [JsonInclude]
        public string SKU { get; private set; } = string.Empty;
        [JsonInclude]
        public string Name { get; private set; } = string.Empty;
        [JsonInclude]
        public decimal Price { get; private set; }
        [JsonInclude]
        public int? Quantity { get; private set; }
        [JsonInclude]
        public string ImagePathUrl { get; private set; } = string.Empty;
        public bool HasQuantity => Quantity != null;
        private List<CartProduct> _cartProducts = [];
        public IEnumerable<CartProduct> CartProducts => _cartProducts;
        public Product(string sku, string name, decimal price, int? quantity, string imagePathUrl)
        {
            SKU = sku;
            Name = name;
            Price = price;
            Quantity = quantity;
            ImagePathUrl = imagePathUrl;
        }
        public Product()
        {
            
        }
        public void DecreaseQuantity(int quantity)
        {
            if(Quantity is null)
            {
                throw new ProductInvalidChangeOfQuantityException();
            }
            if(Quantity < quantity)
            {
                throw new ProductQuantityBelowZeroException();
            }
            Quantity -= quantity;
        }
        public void IncreaseQuantity(int quantity)
        {
            if (Quantity is null)
            {
                throw new ProductInvalidChangeOfQuantityException();
            }
            Quantity += quantity;
        }
    }
}
