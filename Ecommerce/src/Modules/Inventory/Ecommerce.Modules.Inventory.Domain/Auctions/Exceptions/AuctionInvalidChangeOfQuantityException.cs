using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions
{
    internal class AuctionInvalidChangeOfQuantityException : EcommerceException
    {
        public AuctionInvalidChangeOfQuantityException() : base("Cannot change quantity on product that doesn't have quantity.")
        {
        }
    }
}
