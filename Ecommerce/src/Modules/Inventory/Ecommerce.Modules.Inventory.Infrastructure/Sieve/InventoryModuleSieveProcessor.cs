using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.Sieve
{
    public class InventoryModuleSieveProcessor : SieveProcessor
    {
        public InventoryModuleSieveProcessor(IOptions<SieveOptions> options, [FromKeyedServices("inventory-sieve-custom-filters")] ISieveCustomFilterMethods sieveCustomFilterMethods) : base(options, sieveCustomFilterMethods)
        {
        }
        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            return mapper
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
