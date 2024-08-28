using Ecommerce.Shared.Abstractions.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Modules
{
    internal class ModuleClient : IModuleClient
    {
        private readonly IModuleRegistry _moduleRegistry;
        private readonly IModuleSerializer _moduleSerializer;

        public ModuleClient(IModuleRegistry moduleRegistry, IModuleSerializer moduleSerializer)
        {
            _moduleRegistry = moduleRegistry;
            _moduleSerializer = moduleSerializer;
        }
        public async Task PublishAsync(object message)
        {
            var key = message.GetType().Name;
            var registrations = _moduleRegistry
                .GetBroadcastRegistrations(key)
                .Where(r => r.ReceiverType != message.GetType());
            var tasks = new List<Task>();

            foreach (var registration in registrations)
            {
                var action = registration.Action;
                var receiverMessage = TranslateType(message, registration.ReceiverType);
                tasks.Add(action(receiverMessage));
            }

            await Task.WhenAll(tasks);
        }
        private T TranslateType<T>(object value)
            => _moduleSerializer.Deserialize<T>(_moduleSerializer.Serialize(value));

        private object TranslateType(object value, Type type)
            => _moduleSerializer.Deserialize(_moduleSerializer.Serialize(value), type);
    }
}
