using Ecommerce.Modules.Inventory.Application;
using Ecommerce.Modules.Inventory.Infrastructure;
using Ecommerce.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api
{
    internal class InventoryModule : IModule
    {
        public const string BasePath = "inventory-module";
        public string Name { get; } = "Inventory";
        public string Path => BasePath;

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure();
        }

        public void Use(WebApplication app)
        {
            app.MapGet(Path, () =>
            {
                return Results.Ok("Inventory module is working...");
            });
        }
    }
}
