using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Exceptions
{
    internal class OfferAlreadyAcceptedException : EcommerceException
    {
        public OfferAlreadyAcceptedException(int offerId) : base($"Offer: {offerId} was already accepted.")
        {
        }
    }
}
