using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Modules.Orders.Application.Shared.Behaviors;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.Scan(i => i.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(c => c.AssignableTo(typeof(IValidator<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            services.AddMediatR(cfg =>
            {
                cfg.AddOpenBehavior(typeof(OrdersValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(OrdersUnitOfWorkBehavior<,>));
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddSingleton<IOrdersEventMapper, OrdersEventMapper>();
            return services;
        }
    }
}
