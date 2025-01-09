using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Sieve.Configurations
{
    internal class ParameterSieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Manufacturer>(p => p.Id)
                .CanFilter();
            mapper.Property<Manufacturer>(p => p.Name)
                .CanFilter();
            mapper.Property<Manufacturer>(p => p.CreatedAt)
                .CanFilter()
                .CanSort();
        }
    }
}
