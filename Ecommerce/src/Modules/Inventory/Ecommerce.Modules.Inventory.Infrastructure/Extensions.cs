using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Modules.Inventory.Infrastructure.DAL;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Repositories;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddPostgres<InventoryDbContext>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IParameterRepository, ParameterRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            return services;
        }
    }
}
