using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Exceptions
{
    internal class ReviewNotDeletedException : EcommerceException
    {
        public ReviewNotDeletedException() : base("Review was not deleted.")
        {
        }
    }
}
