using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


[assembly: InternalsVisibleTo("Ecommerce.Modules.Inventory.Tests.Unit")]
namespace Ecommerce.Modules.Inventory.Application.Auctions.Exceptions
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
