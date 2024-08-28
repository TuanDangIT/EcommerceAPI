using Ecommerce.Shared.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Messaging.Dispatchers
{
    internal class MessageChannel : IMessageChannel
    {
        private readonly Channel<IMessage> _messages = Channel.CreateUnbounded<IMessage>();

        public ChannelReader<IMessage> Reader => _messages.Reader;
        public ChannelWriter<IMessage> Writer => _messages.Writer;
    }
}
