using Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions;
using Ecommerce.Shared.Abstractions.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class Product : AggregateRoot, IAuditable
    {
        [JsonInclude]
        public new Guid Id { get; private set; }
        [JsonInclude]
        public string SKU { get; private set; } = string.Empty;
        public string? EAN { get; private set; }
        [JsonInclude]
        public string Name { get; private set; } = string.Empty;
        // W następnej wersji WebAPI można dodać obsługiwanie różnych Currency.
        //public string Currency { get; private set; } = string.Empty;
        [JsonInclude]
        public decimal Price { get; private set; }
        public int VAT { get; private set; }
        [JsonInclude]
        public int? Quantity { get; private set; }
        public bool HasQuantity => Quantity != null;
        public bool IsSold => Quantity == 0;
        public int? Reserved { get; private set; }
        public string? Location { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public string? AdditionalDescription { get; private set; }
        public bool IsListed { get; private set; } = false;
        private readonly List<Parameter> _parameters = [];
        public IEnumerable<Parameter> Parameters => _parameters;
        private List<ProductParameter> _productParameters = [];
        public IEnumerable<ProductParameter> ProductParameters => _productParameters;
        public Manufacturer Manufacturer { get; private set; } = new();
        public Guid ManufacturerId { get; private set; }
        public List<Image> _images = [];
        public IEnumerable<Image> Images => _images;
        public Category Category { get; private set; } = new();
        public Guid CategoryId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Product(Guid id, string sku, string name, decimal price, int vat, string description
            , List<ProductParameter> productParameters, Manufacturer manufacturer, Category category
            , List<Image> images, string? ean, int? quantity, string? location, string? additionalDescription, int? reserved)
        {
            IsPriceAndQuantityAndVatValid(price, quantity, vat);
            Id = id;
            SKU = sku;
            EAN = ean;
            Name = name;
            Price = price;
            VAT = vat;
            Quantity = quantity;
            Location = location;
            Description = description;
            AdditionalDescription = additionalDescription;
            _productParameters = productParameters;
            Manufacturer = manufacturer;
            Category = category;
            _images = images;
            Reserved = reserved;
        }
        public Product(string sku, string name, decimal price, int vat, string description
            , List<ProductParameter> productParameters, Manufacturer manufacturer, Category category
            , List<Image> images, string? ean, int? quantity, string? location, string? additionalDescription, int? reserved)
        {
            IsPriceAndQuantityAndVatValid(price, quantity, vat);
            SKU = sku;
            EAN = ean;
            Name = name;
            Price = price;
            VAT = vat;
            Quantity = quantity;
            Location = location;
            Description = description;
            AdditionalDescription = additionalDescription;
            _productParameters = productParameters;
            Manufacturer = manufacturer;
            Category = category;
            _images = images;
            Reserved = reserved;
        }
        public Product()
        {

        }
        public void Purchase(int quantity)
        {
            //DecreaseQuantity(quantity);
            Reserved -= quantity;
            IncrementVersion();
        }
        public void DecreaseQuantity(int quantity)
        {
            CheckIfHasQuantityOrThrow();
            if (Quantity < quantity)
            {
                throw new ProductQuantityBelowZeroException();
            }
            Quantity -= quantity;
            IncrementVersion();
        }
        public void IncreaseQuantity(int quantity)
        {
            CheckIfHasQuantityOrThrow();
            Quantity += quantity;
            IncrementVersion();
        }
        public void Reserve(int quantity)
        {
            CheckIfHasQuantityOrThrow();
            DecreaseQuantity(quantity);
            Reserved += quantity;
        }
        public void Unreserve(int quantity)
        {
            CheckIfHasQuantityOrThrow();
            if (Reserved < quantity)
            {
                throw new ProductReservedBelowZeroException();
            }
            IncreaseQuantity(quantity);
            Reserved -= quantity;
        }
        public void ChangeBaseDetails(string sku, string name, decimal price, int vat, string description, string? ean = null, 
            int? quantity = null, string? location = null, string? additionalDescription = null, int? reserved = null)
        {
            IsPriceAndQuantityAndVatValid(price, quantity, vat);
            SKU = sku;
            EAN = ean;
            Name = name;
            Price = price;
            VAT = vat;
            Quantity = quantity;
            Location = location;
            Description = description;
            AdditionalDescription = additionalDescription;
            Reserved = reserved;
            IncrementVersion();
        }
        public void ChangeManufacturer(Manufacturer manufacturer)
        {
            Manufacturer = manufacturer;
            IncrementVersion();
        }
        public void ChangeCategory(Category category)
        {
            Category = category;
            IncrementVersion();
        }
        public void ChangeImages(List<Image> images)
        {
            _images = images;
            IncrementVersion();
        }
        public void ChangeParameters(List<ProductParameter> parameters)
        {
            _productParameters = parameters;
            IncrementVersion();
        }
        public void ChangePrice(decimal price)
        {
            Price = price;
            IncrementVersion();
        }
        public void ChangeQuantity(int quantity)
        {
            CheckIfHasQuantityOrThrow();
            Quantity = quantity;
            IncrementVersion();
        }
        public void ChangeReservedQuantity(int reserved)
        {
            CheckIfHasQuantityOrThrow();
            Reserved = reserved;
            IncrementVersion();
        }
        private void CheckIfHasQuantityOrThrow()
        {
            if (!HasQuantity)
            {
                throw new ProductInvalidChangeOfQuantityException();
            }
        }
        private void IsPriceAndQuantityAndVatValid(decimal price, int? quantity, int vat)
        {
            if(price < 0)
            {
                throw new ProductPriceBelowZeroException();
            }
            if(quantity < 0)
            {
                throw new ProductQuantityBelowZeroException();
            }
            if (vat < 0)
            {
                throw new ProductVatBelowZeroException();
            }
        }
        private static void IsSkuValid(string sku)
        {
            if (sku.Length >= 8 && sku.Length <= 16) 
            { 
                throw new ProductInvalidSkuException();
            }
        }
        private static void IsEanValid(string ean)
        {
            if (ean.Length == 13)
            {
                throw new ProductInvalidEanException();
            }
        }

        private static void IsNameValid(string name)
        {
            if (name.Length >= 2 && name.Length <= 24)
            {
                throw new ProductInvalidSkuException();
            }
        }

        private static void IsPriceValid(decimal price)
        {
            if (price <= 0)
            {
                throw new ProductInvalidSkuException();
            }
        }

        private static void IsVatValid(int vat)
        {
            if (vat <= 0 && vat <= 100)
            {
                throw new ProductInvalidSkuException();
            }
        }

        private static void IsLocationValid(string location)
        {
            if (location.Length <= 64)
            {
                throw new ProductInvalidSkuException();
            }
        }

        private static void IsQuantityValid(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ProductInvalidSkuException();
            }
        }
    }
}
