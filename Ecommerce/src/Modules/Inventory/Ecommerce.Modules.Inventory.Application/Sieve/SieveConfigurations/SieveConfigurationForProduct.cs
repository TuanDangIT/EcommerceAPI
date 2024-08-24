using Ecommerce.Modules.Inventory.Domain.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Sieve.SieveConfigurations
{
    internal class SieveConfigurationForProduct : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Product>(p => p.SKU)
                .CanFilter();
            mapper.Property<Product>(p => p.EAN)
                .CanFilter();
            mapper.Property<Product>(p => p.Name)
                .CanFilter();
            mapper.Property<Product>(p => p.Price)
                .CanFilter()
                .CanSort();
            mapper.Property<Product>(p => p.Quantity)
                .CanFilter()
                .CanSort();
            mapper.Property<Product>(p => p.Location)
                .CanFilter();
            mapper.Property<Product>(p => p.Description)
                .CanFilter();
            mapper.Property<Product>(p => p.AdditionalDescription)
                .CanFilter();
            mapper.Property<Product>(p => p.Manufacturer.Name)
                .CanFilter();
            mapper.Property<Product>(p => p.Category.Name)
                .CanFilter();
            //mapper.Property<Product>(p => p.Parameters.Select(p => p.Name))
            //    .CanFilter();
            //mapper.Property<Product>(p => p.Parameters)
            //    .CanFilter();
        }
    }
}
