using Ecommerce.Shared.Abstractions.Events;
using Ecommerce.Shared.Abstractions.Modules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Modules
{
    internal static class Extensions
    {
        internal static IServiceCollection AddModuleRequests(this IServiceCollection services,
            IList<Assembly> assemblies)
        {
            services.AddModuleRegistry(assemblies);
            services.AddSingleton<IModuleClient, ModuleClient>();
            services.AddSingleton<IModuleSerializer, JsonModuleSerializer>();
            return services;
        }
        private static void AddModuleRegistry(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var registry = new ModuleRegistry();
            var types = assemblies.SelectMany(x => x.GetTypes()).ToArray();


            var eventTypes = types
                .Where(x => x.IsClass && typeof(IEvent).IsAssignableFrom(x))
                .ToArray();
            //foreach ( var type in eventTypes)
            //{
            //    Console.WriteLine(type.Name);
            //}
            services.AddSingleton<IModuleRegistry>(sp =>
            {
                var eventDispatcher = sp.GetRequiredService<IEventDispatcher>();
                var eventDispatcherType = eventDispatcher.GetType();
                foreach (var type in eventTypes)
                {
                    registry.AddBroadcastAction(type, @event =>
                        (Task)eventDispatcherType.GetMethod(nameof(eventDispatcher.PublishAsync))
                            !.MakeGenericMethod(type)
                            .Invoke(eventDispatcher, new[] { @event })!);

                }

                return registry;
            });
        }
    }
}
