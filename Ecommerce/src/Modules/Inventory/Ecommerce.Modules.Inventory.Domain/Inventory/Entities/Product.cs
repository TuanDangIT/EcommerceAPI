using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string SKU { get; private set; } = string.Empty;
        public string? EAN { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int VAT { get; private set; }
        public int? Quantity { get; private set; }
        public string? Location { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public string? AdditionalDescription { get; private set; }
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
            Quantity -= quantity;
        }
        //public void Edit(string sku, string name, decimal price, int vat, string description
        //    , List<ProductParameter> productParameters, Manufacturer manufacturer, Category category
        //    , List<Image> images, DateTime updatedAt, string? ean = null, int? quantity = null, string? location = null, string? additionalDescription = null)
        //{
        //    SKU = sku;
        //    EAN = ean;
        //    Name = name;
        //    Price = price;
        //    VAT = vat;
        //    Quantity = quantity;
        //    Location = location;
        //    Description = description;
        //    AdditionalDescription = additionalDescription;
        //    _productParameters = productParameters;
        //    Manufacturer = manufacturer;
        //    Category = category;
        //    Images = images;
        //    UpdatedAt = updatedAt;
        //        //}
        //        public void Edit(string sku, string name, decimal price, int vat, string description, List<ProductParameter> productParameters, Manufacturer manufacturer, Category category
        ///*            , List<Image> images*/, DateTime updatedAt, string? ean = null, int? quantity = null, string? location = null, string? additionalDescription = null)
        //        {
        //            SKU = sku;
        //            EAN = ean;
        //            Name = name;
        //            Price = price;
        //            VAT = vat;
        //            Quantity = quantity;
        //            Location = location;
        //            Description = description;
        //            AdditionalDescription = additionalDescription;
        //            _productParameters = productParameters;
        //            Manufacturer = manufacturer;
        //            Category = category;
        //            //Images = images;
        //            UpdatedAt = updatedAt;
        //        }
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
    }
}
