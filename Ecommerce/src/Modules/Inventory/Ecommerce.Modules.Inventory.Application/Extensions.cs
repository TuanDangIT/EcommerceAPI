using Ecommerce.Modules.Inventory.Application.Inventory.Services;
using Ecommerce.Modules.Inventory.Application.Shared.Behaviors;
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
            services.Scan(i => i.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            services.AddMediatR(cfg =>
            {
                cfg.AddOpenBehavior(typeof(InventoryValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(InventoryUnitOfWorkBehavior<,>));
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddSingleton<IInventoryEventMapper, InventoryEventMapper>();
            return services;
        }
    }
}
