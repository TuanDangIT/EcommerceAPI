using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Postgres
{
    public static class Extensions
    {
        private const string _postgresOptionsSectionName = "Postgres";
        internal static IServiceCollection AddPostgres(this IServiceCollection services)
        {
            var options = services.GetOptions<PostgresOptions>(_postgresOptionsSectionName);
            services.AddSingleton(options);
            return services;
        }
        public static IServiceCollection AddPostgres<T>(this IServiceCollection services) where T : DbContext
        {
            var options = services.GetOptions<PostgresOptions>("Postgres");
            services.AddDbContext<T>(x => x.UseNpgsql(options.ConnectionString));
            return services;
        }
    }
}
