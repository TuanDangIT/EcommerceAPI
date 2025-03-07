using Ecommerce.Modules.Inventory.Application.DAL;
using Ecommerce.Modules.Inventory.Application.Shared.Abstractions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Modules.Inventory.Infrastructure.CsvHelper.Services;
using Ecommerce.Modules.Inventory.Infrastructure.DAL;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Repositories;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.UnitOfWork;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ecommerce.Modules.Inventory.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddPostgres<InventoryDbContext>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IParameterRepository, ParameterRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IInventoryUnitOfWork, InventoryUnitOfWork>();
            services.AddSingleton<ICsvService, CsvService>();
            return services;
        }
    }
}
