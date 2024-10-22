using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Events
{
    public sealed record class CustomerActivated(Guid CustomerId, string Email, string FirstName, string LastName) : IEvent;
}
