using Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class Product
    {
        [JsonInclude]
        public Guid Id { get; private set; }
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
            , List<Image> images, DateTime createdAt, string? ean = null, int? quantity = null, string? location = null, string? additionalDescription = null)
        {
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
            CreatedAt = createdAt;
        }
        public Product()
        {

        }
        public void DecreaseQuantity(int quantity)
        {
            if (Quantity is null)
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
        public void ChangeBaseDetails(string sku, string name, decimal price, int vat, string description, string? ean = null, int? quantity = null, string? location = null, string? additionalDescription = null)
        {
            SKU = sku;
            EAN = ean;
            Name = name;
            Price = price;
            VAT = vat;
            Quantity = quantity;
            Location = location;
            Description = description;
            AdditionalDescription = additionalDescription;
        }
        public void ChangeManufacturer(Manufacturer manufacturer)
            => Manufacturer = manufacturer;
        public void ChangeCategory(Category category)
            => Category = category;
        public void ChangeImages(List<Image> images)
            => _images = images;
        public void ChangeProductParameters(List<ProductParameter> parameters)
            => _productParameters = parameters;
        public void SetUpdateAt(DateTime updateAt)
            => UpdatedAt = updateAt;
        public void List()
            => IsListed = true;
        public void Unlist()
            => IsListed = false;
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
