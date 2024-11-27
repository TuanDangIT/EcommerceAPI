using Ecommerce.Shared.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Messaging.Dispatchers
{
    internal class AsyncMessageDispatcher : IAsyncMessageDispatcher
    {
        private readonly IMessageChannel _messageChannel;

        public AsyncMessageDispatcher(IMessageChannel messageChannel)
            => _messageChannel = messageChannel;
        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage
            => await _messageChannel.Writer.WriteAsync(message);
    }
}
