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
        public int Quantity { get; private set; }
        [JsonInclude]
        public string? ImagePathUrl { get; private set; }
        public ReturnProduct(string sku, string name, decimal price, int quantity, string? imagePathUrl)
        {
            if(quantity <= 0)
            {
                throw new ReturnProductQuantityBelowZeroOrEqualException();
            }
            if(price < 0)
            {
                throw new ReturnProductPriceBelowZeroException();
            }
            SKU = sku;
            Name = name;
            Price = price;
            Quantity = quantity;
            ImagePathUrl = imagePathUrl;
        }
        public ReturnProduct()
        {

        }
    }
}
