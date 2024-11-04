using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Services;
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
        public static IServiceCollection AddPagination(this IServiceCollection services)
        {
            services.AddSingleton<IFilterService, FilterService>();
            return services;
        }
    }
}
