using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions
{
    internal class AuctionQuantityBelowZeroException : EcommerceException
    {
        public AuctionQuantityBelowZeroException() : base("Auction's quantity cannot be below zero.")
        {
        }
    }
}
