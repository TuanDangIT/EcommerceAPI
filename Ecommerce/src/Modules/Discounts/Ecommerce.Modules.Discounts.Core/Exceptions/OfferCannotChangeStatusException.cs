using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Exceptions
{
    internal class OfferCannotChangeStatusException : EcommerceException
    {
        public OfferCannotChangeStatusException(string message) : base(message)
        {
        }
    }
}
