using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Events
{
    public sealed record class ComplaintSubmitted(Guid OrderId, Guid? CustomerId, string FirstName, string Email, string Title, string Description, DateTime CreatedAt) : IDomainEvent;
}
