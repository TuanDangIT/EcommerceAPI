using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Infrastructure.Api;
using Ecommerce.Shared.Infrastructure.Auth;
using Ecommerce.Shared.Infrastructure.Company;
using Ecommerce.Shared.Infrastructure.Contexts;
using Ecommerce.Shared.Infrastructure.DomainEvents;
using Ecommerce.Shared.Infrastructure.Events;
using Ecommerce.Shared.Infrastructure.Exceptions;
using Ecommerce.Shared.Infrastructure.InPost;
using Ecommerce.Shared.Infrastructure.Mails;
using Ecommerce.Shared.Infrastructure.Messaging;
using Ecommerce.Shared.Infrastructure.Modules;
using Ecommerce.Shared.Infrastructure.Pagination;
using Ecommerce.Shared.Infrastructure.Postgres;
using Ecommerce.Shared.Infrastructure.RateLimiter;
using Ecommerce.Shared.Infrastructure.Serilog;
using Ecommerce.Shared.Infrastructure.BackgroundServices;
using Ecommerce.Shared.Infrastructure.Storage;
using Ecommerce.Shared.Infrastructure.Stripe;
using Ecommerce.Shared.Infrastructure.Swagger;
using Ecommerce.Shared.Infrastructure.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination.Sieve;
using Ecommerce.Shared.Infrastructure.Carts;

[assembly: InternalsVisibleTo("Ecommerce.Bootstrapper")]
namespace Ecommerce.Shared.Infrastructure
{
    internal static class Extensions
    {
        public static ConfigureHostBuilder ConfigureHost(this ConfigureHostBuilder host)
        {
            host.ConfigureSerilog();
            return host;
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IList<Assembly> assemblies, IConfiguration configuration)
        {
            services.AddErrorHandling();
            services.AddEvents(assemblies);
            services.AddDomainEvents(assemblies);
            services.AddModuleRequests(assemblies);
            services.AddMessaging();
            services.AddContext();
            services.AddSingleton(TimeProvider.System);
            services.AddAuth();
            services.AddPostgres();
            services.AddPagination(configuration);
            services.AddStripe();
            services.AddInpost();
            services.AddCartOptions();
            services.AddCompanyDetails();
            services.AddMails();
            services.AddAzureBlobStorage();
            services.AddHostedService<AppInitializer>();
            services.AddControllers()
                .ConfigureApplicationPartManager(manager =>
                {
                    foreach (var part in manager.ApplicationParts)
                    {
                        Console.WriteLine(part.Name);
                    }
                    Console.WriteLine("--------------");
                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
                })
            .AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            services.AddEndpointsApiExplorer();
            services.AddVersioning();
            services.AddDocumentation();
            services.AddAspRateLimiter();
            return services;
        }
        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDocumentation();
            }
            app.UseErrorHandling();
            app.MapGet("api", () =>
            {
                return Results.Ok("Ecommerce API is working!");
            });
            app.UseHttpsRedirection();
            app.UseAuth();
            app.UseAspRateLimiter();
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
