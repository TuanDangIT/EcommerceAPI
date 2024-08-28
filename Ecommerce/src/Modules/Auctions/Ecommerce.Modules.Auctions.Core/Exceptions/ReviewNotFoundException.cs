using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.Exceptions
{
    internal class ReviewNotFoundException : EcommerceException
    {
        public Guid Id { get; }
        public ReviewNotFoundException(Guid id) : base($"Review: {id} was not found.")
        {
            Id = id;
        }
    }
}
