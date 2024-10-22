using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Events.Externals
{
    public sealed record class ComplaintRejected(Guid ComplaintId, Guid OrderId, Guid? CustomerId, string FirstName, string Email, string Title, string Decision,
        string? AdditionalInformation, DateTime CreatedAt) : IEvent;
}
