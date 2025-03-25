using Ecommerce.Modules.Inventory.Infrastructure.Sieve.Filters;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.Sieve
{
    internal static class Extensions
    {
        public static IServiceCollection AddSieve(this IServiceCollection services)
        {
            services.AddKeyedScoped<ISieveProcessor, InventoryModuleSieveProcessor>("inventory-sieve-processor");
            services.AddKeyedScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>("inventory-sieve-custom-filters");
            return services;
        }
    }
}
