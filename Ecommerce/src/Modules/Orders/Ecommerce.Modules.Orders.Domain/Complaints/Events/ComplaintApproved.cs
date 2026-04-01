using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Complaints.Events
{
    public sealed record class ComplaintApproved(Guid OrderId, decimal RefundedAmount) : IDomainEvent;
}
