using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions
{
    internal class AuctionsNotDeletedException : EcommerceException
    {
        public AuctionsNotDeletedException() : base("One or more auctions were not unlisted.")
        {
        }
    }
}
