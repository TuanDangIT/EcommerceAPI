using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class Product : BaseEntity<int>       //OrderItem could be a better name.
    {
        [JsonInclude]
        public string SKU { get; private set; } = string.Empty;
        [JsonInclude]
        public string Name { get; private set; } = string.Empty;
        [JsonInclude]
        public decimal Price { get; private set; }
        [JsonInclude]
        public int Quantity { get; private set; }
        [JsonInclude]
        public string ImagePathUrl { get; private set; } = string.Empty;
        public Product(string sku, string name, decimal price, int quantity, string imagePathUrl)
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
            if(Quantity < quantity)
            {
                throw new ProductQuantityBelowZeroException();
            }
            Quantity -= quantity;
        }
    }
}
