using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Returns.Exception;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Entities
{
    public class ReturnProduct : BaseEntity<int>
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
        public ReturnProductStatus Status { get; private set; }
        [JsonInclude]
        public string? ImagePathUrl { get; private set; }
        public ReturnProduct(string sku, string name, decimal price, decimal unitPrice, int quantity, string? imagePathUrl)
        {
            if(quantity <= 0)
            {
                throw new ReturnProductQuantityBelowZeroOrEqualException();
            }
            if(price < 0)
            {
                throw new ReturnProductPriceBelowZeroException();
            }
            if (unitPrice < 0)
            {
                throw new ReturnProductUnitPriceBelowZeroException();
            }
            SKU = sku;
            Name = name;
            Price = price;
            UnitPrice = unitPrice;
            Quantity = quantity;
            ImagePathUrl = imagePathUrl;
        }
        public ReturnProduct(string sku, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ReturnProductQuantityBelowZeroOrEqualException();
            }
            SKU = sku;
            Quantity = quantity;
        }
        public ReturnProduct()
        {

        }
        public void SetStatus(ReturnProductStatus status)
            => Status = status; 
        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
            CalculatePrice();
        }
        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
            CalculatePrice();
        }
        private void CalculatePrice()
            => Price = UnitPrice * Quantity;
    }
}
