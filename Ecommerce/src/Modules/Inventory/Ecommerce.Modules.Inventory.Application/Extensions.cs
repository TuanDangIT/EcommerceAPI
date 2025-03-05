using Ecommerce.Modules.Inventory.Application.Inventory.Services;
using Ecommerce.Modules.Inventory.Application.Inventory.Sieve.Filters;
using Ecommerce.Modules.Inventory.Application.Shared.Behaviors;
using Ecommerce.Modules.Inventory.Application.Shared.Sieve;
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
        private const string _sieveSectionName = "Sieve";
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.AddOpenBehavior(typeof(InventoryValidationBehavior<,>));
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddSingleton<IInventoryEventMapper, InventoryEventMapper>();
            services.Scan(i => i.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            services.Configure<SieveOptions>(configuration.GetSection(_sieveSectionName));
            services.AddKeyedScoped<ISieveProcessor, InventoryModuleSieveProcessor>("inventory-sieve-processor");
            services.AddKeyedScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>("inventory-sieve-custom-filters");
            return services;
        }
    }
}
