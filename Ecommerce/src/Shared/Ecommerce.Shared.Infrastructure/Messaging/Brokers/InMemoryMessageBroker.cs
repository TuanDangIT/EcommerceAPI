using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Shared.Abstractions.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Messaging.Brokers
{
    public class InMemoryMessageBroker : IMessageBroker
    {
        //private readonly IModuleClient _moduleClient;
        private readonly IAsyncMessageDispatcher _asyncMessageDispatcher;

        public InMemoryMessageBroker(IAsyncMessageDispatcher asyncMessageDispatcher)
        {
            _asyncMessageDispatcher = asyncMessageDispatcher;
        }
        public async Task PublishAsync(params IMessage[] messages)
        {
            if (messages is null)
            {
                return;
            }

            messages = messages.Where(x => x is not null).ToArray();

            if (!messages.Any())
            {
                return;
            }

            var tasks = new List<Task>();

            foreach (var message in messages)
            {
                await _asyncMessageDispatcher.PublishAsync(message);
                //tasks.Add(_moduleClient.PublishAsync(message));
            }

            await Task.WhenAll(tasks);
        }
    }
}
