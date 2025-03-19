using Ecommerce.Shared.Infrastructure.Stripe;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Pagination.CursorPagination
{
    internal static class Extensions
    {
        private const string _cursorPaginationOptionsSectionName = "CursorPagination";
        public static IServiceCollection AddCursorPagination(this IServiceCollection services)
        {
            var cursorPaginationOptions = services.GetOptions<CursorPaginationOptions>(_cursorPaginationOptionsSectionName);
            services.AddSingleton(cursorPaginationOptions);
            return services;
        }
    }
}
