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
    public class Product : BaseEntity, IAuditable
    {
        [JsonInclude]
        public string SKU { get; private set; } = string.Empty;
        [JsonInclude]
        public string Name { get; private set; } = string.Empty;
        [JsonInclude]
        public decimal Price { get; private set; }
        [JsonInclude]
        public int? Quantity { get; private set; }
        public bool HasQuantity => Quantity.HasValue;
        [JsonInclude]
        public string? ImagePathUrl { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Product(Guid id, string sku, string name, decimal price, int? quantity, string? imagePathUrl)
        {
            if (quantity < 0)
            {
                throw new ProductQuantityBelowZeroException();
            }
            if (price < 0)
            {
                throw new ProductPriceBelowZeroException();
            }
            Id = id;
            SKU = sku;
            Name = name;
            Price = price;
            Quantity = quantity;
            ImagePathUrl = imagePathUrl;
        }

        public Product(string sku, string name, decimal price, int? quantity, string? imagePathUrl)
        {
            if (quantity < 0)
            {
                throw new ProductQuantityBelowZeroException();
            }
            if (price < 0)
            {
                throw new ProductPriceBelowZeroException();
            }
            SKU = sku;
            Name = name;
            Price = price;
            Quantity = quantity;
            ImagePathUrl = imagePathUrl;
        }

        private Product()
        {
            
        }

        public void DecreaseQuantity(int quantity)
        {
            if (HasQuantity)
            {
                if (quantity > Quantity)
                {
                    throw new ProductQuantityBelowZeroException();
                }
                Quantity -= quantity;
            }
        }

        public void IncreaseQuantity(int quantity)
        {
            if (HasQuantity)
            {
                Quantity += quantity;
            }
        }
    }
}
