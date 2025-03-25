using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Sieve
{
    internal static class Extensions
    {
        public static IServiceCollection AddSieve(this IServiceCollection services)
        {
            services.AddKeyedScoped<ISieveProcessor, OrdersModuleSieveProcessor>("orders-sieve-processor");
            return services;
        }
    }
}
