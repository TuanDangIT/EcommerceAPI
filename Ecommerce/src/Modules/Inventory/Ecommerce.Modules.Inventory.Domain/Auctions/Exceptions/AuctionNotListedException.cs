using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions
{
    internal class AuctionNotListedException : EcommerceException
    {
        public AuctionNotListedException() : base("One or more auctions were not listed.")
        {
        }
    }
}
