using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Infrastructure.Api;
using Ecommerce.Shared.Infrastructure.Auth;
using Ecommerce.Shared.Infrastructure.Contexts;
using Ecommerce.Shared.Infrastructure.Exceptions;
using Ecommerce.Shared.Infrastructure.MediatR;
using Ecommerce.Shared.Infrastructure.Postgres;
using Ecommerce.Shared.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Ecommerce.Bootstrapper")]
namespace Ecommerce.Shared.Infrastructure
{
    internal static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddErrorHandling();
            services.AddMediatR();
            services.AddContext();
            services.AddSingleton(TimeProvider.System);
            services.AddAuth();
            services.AddContext();
            services.AddPostgres();
            services.AddHostedService<AppInitializer>();
            services.AddControllers()
                .ConfigureApplicationPartManager(manager =>
                {
                    foreach (var part in manager.ApplicationParts)
                    {
                        Console.WriteLine(part.Name);
                    }
                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
                });
            //services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();
            return services;
        }
        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            app.UseErrorHandling();
            app.MapGet("api", () =>
            {
                return Results.Ok("Ecommerce API is working!");
            });
            app.UseHttpsRedirection();
            app.MapControllers();
            return app;
        }
        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetOptions<T>(sectionName);
        }

        public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var options = new T();
            configuration.GetSection(sectionName).Bind(options);
            return options;
        }
    }
}
