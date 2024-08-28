using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Messaging
{
    public interface IAsyncMessageDispatcher
    {
        Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage;
    }
}
