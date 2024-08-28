using Ecommerce.Modules.Auctions.Core;
using Ecommerce.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Api
{
    internal class AuctionsModule : IModule
    {
        public const string BasePath = "auctions-module";
        public string Name { get; } = "Auctions";
        public string Path => BasePath;

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCore(configuration);
        }

        public void Use(WebApplication app)
        {
            app.MapGet(Path, () =>
            {
                return Results.Ok("Auctions module is working...");
            });
        }
    }
}
