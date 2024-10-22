using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Events.Externals
{
    public sealed record class OfferAccepted(int OfferId, Guid CustomerId, string SKU, string ProductName, string Code, decimal OfferedPrice, decimal OldPrice, DateTime ExpiresAt) : IEvent;
}
