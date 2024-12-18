﻿using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Modules.Inventory.Infrastructure.DAL;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Repositories;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.UnitOfWork;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
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
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddPostgres<InventoryDbContext>();
            services.AddScoped<IInventoryUnitOfWork, InventoryUnitOfWork>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IParameterRepository, ParameterRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            return services;
        }
    }
}
