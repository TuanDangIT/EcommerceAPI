using Ecommerce.Modules.Products.Core.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Sieve.SieveConfiguration
{
    internal class SieveConfigurationForProduct : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Product>(p => p.SKU)
                .CanFilter();
            mapper.Property<Product>(p => p.Name)
                .CanFilter();
            mapper.Property<Product>(p => p.Price)
                .CanFilter()
                .CanSort();
            mapper.Property<Product>(p => p.Quantity)
                .CanFilter()
                .CanSort();
            mapper.Property<Product>(p => p.Description)
                .CanFilter();
            mapper.Property<Product>(p => p.AdditionalDescription)
                .CanFilter();
            mapper.Property<Product>(p => p.Manufacturer)
                .CanFilter();
            mapper.Property<Product>(p => p.Category)
                .CanFilter();
        }
    }
}
