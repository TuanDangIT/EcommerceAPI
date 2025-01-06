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
        public decimal UnitPrice { get; private set; }
        [JsonInclude]
        public int Quantity { get; private set; }
        [JsonInclude]
        public string? ImagePathUrl { get; private set; }
        public Product(string sku, string name, decimal price, decimal unitPrice, int quantity, string? imagePathUrl)
        {
            if(unitPrice < 0)
            {
                throw new ProductUnitPriceBelowZeroException();
            }
            if(price < 0)
            {
                throw new ProductPriceBelowZeroException();
            }
            if(quantity <= 0)
            {
                throw new ProductQuantityBelowOrEqualZeroException();
            }
            SKU = sku;
            Name = name;
            Price = price;
            UnitPrice = unitPrice;
            Quantity = quantity;
            ImagePathUrl = imagePathUrl;
        }
        public Product(string sku, string name, decimal unitPrice, int quantity, string? imagePathUrl)
        {
            SKU = sku;
            Name = name;
            Price = unitPrice * quantity;
            UnitPrice = unitPrice;
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
            CalculatePrice();
        }
        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
            CalculatePrice();
        }
        public void EditUnitPrice(decimal unitPrice)
        {
            UnitPrice = unitPrice;
            CalculatePrice();
        }
        private void CalculatePrice()
            => Price = UnitPrice * Quantity;
    }
}
