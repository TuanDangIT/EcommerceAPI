using Ecommerce.Modules.Products.Core.DAL;
using Ecommerce.Modules.Products.Core.Services;
using Ecommerce.Modules.Products.Core.Sieve;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ecommerce.Modules.Products.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPostgres<ProductDbContext>();
            services.AddSingleton<IProductDbContext>(sp => sp.GetRequiredService<ProductDbContext>());
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.Configure<SieveOptions>(configuration.GetSection("Sieve"));
            services.AddScoped<ISieveProcessor, ProductModuleSieveProcessor>();
            return services;
        }
    }
}
