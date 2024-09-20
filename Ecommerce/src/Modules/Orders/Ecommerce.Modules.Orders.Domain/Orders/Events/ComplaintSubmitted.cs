using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Events
{
    public sealed record class ComplaintSubmitted(string Title, string Description, Customer Customer, Order Order, DateTime CreatedAt) : IDomainEvent;
}
