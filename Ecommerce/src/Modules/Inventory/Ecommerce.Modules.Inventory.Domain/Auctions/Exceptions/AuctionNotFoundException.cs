using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions
{
    internal class AuctionNotFoundException : EcommerceException
    {
        public AuctionNotFoundException(Guid auctionId) : base($"Auction: {auctionId} was not found.")
        {
        }
    }
}
