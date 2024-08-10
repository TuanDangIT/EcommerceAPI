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

namespace Ecommerce.Modules.Returns.Api
{
    internal class Returns : IModule
    {
        public const string BasePath = "returns-module";
        public string Name { get; } = "Returns";
        public string Path => BasePath;

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
        }

        public void Use(WebApplication app)
        {
            app.MapGet(Path, () =>
            {
                return Results.Ok("Returns module is working...");
            });
        }
    }
}
