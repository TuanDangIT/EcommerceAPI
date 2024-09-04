using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions
{
    internal class ProductsCannotBeRelistedException : EcommerceException
    {
        public Guid[] Ids { get; set; }
        public ProductsCannotBeRelistedException(Guid[] ids) : base($"Auction: {string.Join(", ", ids)} cannot be relisted.")
        {
            Ids = ids;
        }
    }
}
