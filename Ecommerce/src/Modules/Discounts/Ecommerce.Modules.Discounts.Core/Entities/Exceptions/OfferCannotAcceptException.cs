using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities.Exceptions
{
    internal class OfferCannotAcceptException(int offerId) : EcommerceException($"Cannot accept offer: {offerId} more than once.")
    {
    }
}
