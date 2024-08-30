using Ecommerce.Modules.Carts.Core;
using Ecommerce.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Api
{
    internal class CartsModule : IModule
    {
        public const string BasePath = "carts-module";
        public string Name { get; } = "Carts";
        public string Path => BasePath;

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCore();
        }

        public void Use(WebApplication app)
        {
            app.MapGet(Path, () =>
            {
                return Results.Ok("Carts module is working...");
            });
        }
    }
}
