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
            mapper.Property<Category>(c => c.Name)
                .CanFilter();
            mapper.Property<Category>(c => c.CreatedAt)
                .CanSort();
        }
    }
}
