using Ecommerce.Shared.Abstractions.Events;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Events
{
    public sealed record class InvoiceCreated(Guid OrderId, Guid? CustomerId, string FirstName, string Email, string InvoiceNo) : IEvent;
}
