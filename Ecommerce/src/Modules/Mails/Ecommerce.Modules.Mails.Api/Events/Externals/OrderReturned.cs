using Ecommerce.Modules.Mails.Api.Entities.ValueObjects;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Events.Externals
{
    public sealed record class OrderReturned(Guid OrderId, Guid? CustomerId, string FirstName, string Email, IEnumerable<Product> Products) : IEvent;
}
