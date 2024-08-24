using Ecommerce.Modules.Inventory.Domain.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Sieve.SieveConfigurations
{
    internal class SieveConfigurationForManufacturer : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Manufacturer>(c => c.Name)
                .CanFilter();
            mapper.Property<Manufacturer>(c => c.CreatedAt)
                .CanSort();
        }
    }
}
