﻿using Ecommerce.Modules.Orders.Application;
using Ecommerce.Modules.Orders.Domain;
using Ecommerce.Modules.Orders.Infrastructure;
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

namespace Ecommerce.Modules.Orders.Api
{
    internal class OrdersModule : IModule
    {
        public const string BasePath = "orders-module";
        public string Name { get; } = "Orders";
        public string Path => BasePath;

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDomain();
            services.AddApplication();
            services.AddInfrastructure();
        }

        public void Use(WebApplication app)
        {
            app.MapGet(Path, () =>
            {
                return Results.Ok("Orders module is working...");
            });
        }
    }
}
