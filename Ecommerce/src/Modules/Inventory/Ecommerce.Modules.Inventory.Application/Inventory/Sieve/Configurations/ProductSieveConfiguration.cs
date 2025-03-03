using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Sieve.Configurations
{
    internal class ProductSieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Product>(p => p.Id)
                .CanFilter();
            mapper.Property<Product>(p => p.SKU)
                .CanFilter();
            mapper.Property<Product>(p => p.EAN)
                .CanFilter();
            mapper.Property<Product>(p => p.Name)
                .CanFilter();
            mapper.Property<Product>(p => p.Price)
                .CanFilter()
                .CanSort();
            mapper.Property<Product>(p => p.VAT)
                .CanFilter()
                .CanSort();
            mapper.Property<Product>(p => p.Quantity)
                .CanFilter()
                .CanSort();
            mapper.Property<Product>(p => p.IsSold)
                .CanFilter();
            mapper.Property<Product>(p => p.HasQuantity)
                .CanFilter();
            mapper.Property<Product>(p => p.Location)
                .CanFilter();
            mapper.Property<Product>(p => p.Description)
                .CanFilter();
            mapper.Property<Product>(p => p.AdditionalDescription)
                .CanFilter();
            mapper.Property<Product>(p => p.IsListed)
                .CanFilter();
            mapper.Property<Product>(p => p.Manufacturer.Name)
                .CanFilter();
            mapper.Property<Product>(p => p.Category.Name)
                .CanFilter();
            mapper.Property<Product>(p => p.CreatedAt)
                .CanFilter()
                .CanSort();
        }
    }
}
