using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Services
{
    public interface IOrdersEventMapper
    {
        //Task<IMessage> MapAsync(IDomainEvent @event);
        IMessage Map(IDomainEvent @event);
    }
}
