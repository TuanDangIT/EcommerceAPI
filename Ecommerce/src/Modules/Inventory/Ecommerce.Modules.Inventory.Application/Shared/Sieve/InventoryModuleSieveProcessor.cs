using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Shared.Sieve
{
    public class InventoryModuleSieveProcessor : SieveProcessor
    {
        public InventoryModuleSieveProcessor(IOptions<SieveOptions> options, ISieveCustomFilterMethods sieveCustomFilterMethods) : base(options, sieveCustomFilterMethods)
        {
        }
        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            return mapper
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
