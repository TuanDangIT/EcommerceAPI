using Ecommerce.Shared.Infrastructure.Stripe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination.Sieve
{
    internal static class Extensions
    {
        private const string _sieveOptionsSectionName = "Sieve";
        public static IServiceCollection AddSieve(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SieveOptions>(configuration.GetSection(_sieveOptionsSectionName));
            return services;
        }
    }
}
