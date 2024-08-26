using Ecommerce.Modules.Products.Core.DAL;
using Ecommerce.Modules.Products.Core.Services;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddPostgres<ProductDbContext>();
            services.AddSingleton<IProductDbContext>(sp => sp.GetRequiredService<ProductDbContext>());
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IReviewService, ReviewService>();
            return services;
        }
    }
}
