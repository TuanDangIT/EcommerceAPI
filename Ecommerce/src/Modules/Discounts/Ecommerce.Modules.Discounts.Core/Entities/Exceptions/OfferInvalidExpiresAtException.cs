using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities.Exceptions
{
    internal class OfferInvalidExpiresAtException(DateTime expiresAt) : EcommerceException($"Expires at time: {expiresAt.ToString("dd-MM-yyyy")} is invalid")
    {
    }
}
