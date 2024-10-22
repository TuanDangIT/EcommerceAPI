using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Events
{
    public sealed record class ReturnRejected(Guid ReturnId, Guid OrderId, Guid? CustomerId, string FirstName, string Email, string RejectReason, IEnumerable<object> Products, DateTime CreatedAt) : IEvent;
}
