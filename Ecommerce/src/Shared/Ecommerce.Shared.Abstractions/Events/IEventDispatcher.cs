using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Events
{
    public interface IEventDispatcher
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
    }
}
