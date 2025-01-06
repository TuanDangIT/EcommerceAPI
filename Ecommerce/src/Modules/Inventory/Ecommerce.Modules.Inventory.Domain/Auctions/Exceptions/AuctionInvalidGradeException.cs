using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions
{
    internal class AuctionInvalidGradeException : EcommerceException
    {
        public AuctionInvalidGradeException() : base("Auction's grade must be between 1 and 10.")
        {
        }
    }
}
