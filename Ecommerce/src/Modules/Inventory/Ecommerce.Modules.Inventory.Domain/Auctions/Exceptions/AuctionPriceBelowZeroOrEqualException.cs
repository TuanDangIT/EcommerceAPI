using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions
{
    internal class AuctionPriceBelowZeroOrEqualException : EcommerceException
    {
        public AuctionPriceBelowZeroOrEqualException() : base("Auction's price must be higher than 0.")
        {
        }
    }
}
