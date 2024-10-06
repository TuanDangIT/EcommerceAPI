using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Events
{
    public sealed record class OfferRequested(string SKU, decimal Price, decimal OldPrice, string Reason, Guid CustomerId) : IEvent;
}
