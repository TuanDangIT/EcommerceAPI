using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.Exceptions
{
    public sealed class PaginationException : EcommerceException
    {
        public PaginationException() : base("Pagination went wrong.")
        {
        }
    }
}
