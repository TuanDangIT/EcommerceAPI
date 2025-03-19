using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Services;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination.Sieve;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Pagination
{
    internal static class Extensions
    {
        public static IServiceCollection AddPagination(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSieve(configuration);
            services.AddCursorPagination();
            services.AddSingleton<IFilterService, FilterService>();
            return services;
        }
    }
}
