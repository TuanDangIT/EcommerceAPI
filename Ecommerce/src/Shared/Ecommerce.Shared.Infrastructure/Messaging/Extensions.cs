using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Shared.Infrastructure.Messaging.Brokers;
using Ecommerce.Shared.Infrastructure.Messaging.Dispatchers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Messaging
{
    internal static class Extensions
    {
        internal static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBroker, InMemoryMessageBroker>();
            services.AddSingleton<IMessageChannel, MessageChannel>();
            services.AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>();
            services.AddHostedService<BackgroundDispatcher>();
            return services;
        }
    }
}
