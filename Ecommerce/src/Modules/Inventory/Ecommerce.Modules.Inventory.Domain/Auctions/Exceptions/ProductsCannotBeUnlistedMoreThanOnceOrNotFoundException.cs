using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions
{
    internal class ProductsCannotBeUnlistedMoreThanOnceOrNotFoundException : EcommerceException
    {
        public Guid[] Ids { get; set; }
        public ProductsCannotBeUnlistedMoreThanOnceOrNotFoundException(Guid[] ids) : base($"Auction: {string.Join(", ", ids)} cannot be unlisted more than once or were not found.")
        {
            Ids = ids;
        }
    }
}
