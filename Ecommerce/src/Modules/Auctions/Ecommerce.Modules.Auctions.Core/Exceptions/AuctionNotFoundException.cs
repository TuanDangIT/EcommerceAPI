using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.Exceptions
{
    public class AuctionNotFoundException : EcommerceException
    {
        public Guid Id { get; }
        public AuctionNotFoundException(Guid id) : base($"Auction: {id} was not found.")
        {
            Id = id;
        }
    }
}
