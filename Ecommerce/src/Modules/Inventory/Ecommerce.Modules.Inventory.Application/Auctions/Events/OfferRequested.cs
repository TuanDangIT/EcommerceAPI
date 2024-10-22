using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Events
{
    public sealed record class OfferRequested(Guid CustomerId, string SKU, string ProductName, decimal OfferedPrice, decimal OldPrice, string Reason) : IEvent;
}
