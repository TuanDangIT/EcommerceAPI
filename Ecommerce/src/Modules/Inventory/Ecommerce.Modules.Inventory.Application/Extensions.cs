using Ecommerce.Modules.Inventory.Application.Behavior;
using Ecommerce.Modules.Inventory.Application.Features.Parameters.ChangeParameterName;
using Ecommerce.Modules.Inventory.Application.Sieve;
using Ecommerce.Modules.Inventory.Application.Sieve.CustomFilters;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.Scan(i => i.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            services.Configure<SieveOptions>(configuration.GetSection("Sieve"));
            services.AddScoped<ISieveProcessor, InventoryModuleSieveProcessor>();
            services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();
            return services;
        }
    }
}
