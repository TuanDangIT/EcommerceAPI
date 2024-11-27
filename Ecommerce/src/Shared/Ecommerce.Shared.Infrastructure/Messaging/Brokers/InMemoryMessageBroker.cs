using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Shared.Abstractions.Modules;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Messaging.Brokers
{
    public class InMemoryMessageBroker : IMessageBroker
    {
        private readonly IAsyncMessageDispatcher _asyncMessageDispatcher;
        private readonly ILogger<InMemoryMessageBroker> _logger;

        public InMemoryMessageBroker(IAsyncMessageDispatcher asyncMessageDispatcher, ILogger<InMemoryMessageBroker> logger)
        {
            _asyncMessageDispatcher = asyncMessageDispatcher;
            _logger = logger;
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
            foreach (var message in messages)
            {
                _logger.LogDebug("Published integration event: {message} to background dispatcher.", message);
                await _asyncMessageDispatcher.PublishAsync(message);
            }
        }
    }
}
