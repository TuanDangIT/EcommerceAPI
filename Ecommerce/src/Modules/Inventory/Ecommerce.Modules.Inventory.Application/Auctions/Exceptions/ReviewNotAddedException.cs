using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Exceptions
{
    internal class ReviewNotAddedException : EcommerceException
    {
        public ReviewNotAddedException() : base("Review was not created.")
        {
        }
    }
}
