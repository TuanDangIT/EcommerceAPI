using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Sieve.Configurations
{
    internal class ManufacturerSieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Manufacturer>(c => c.Id)
                .CanFilter();
            mapper.Property<Manufacturer>(c => c.Name)
                .CanFilter();
            mapper.Property<Manufacturer>(c => c.CreatedAt)
                .CanFilter()
                .CanSort();
        }
    }
}
