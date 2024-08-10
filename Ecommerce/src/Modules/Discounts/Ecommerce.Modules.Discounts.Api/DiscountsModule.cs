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

namespace Ecommerce.Modules.Discounts.Api
{
    internal class DiscountsModule : IModule
    {
        public const string BasePath = "discounts-module";
        public string Name { get; } = "Discounts";
        public string Path => BasePath;

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
        }

        public void Use(WebApplication app)
        {
            app.MapGet(Path, () =>
            {
                return Results.Ok("Discounts module is working...");
            });
        }
    }
}
