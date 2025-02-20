using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Ecommerce.Modules.Discounts.Tests.Unit")]
namespace Ecommerce.Modules.Discounts.Core.Entities.Exceptions
{
    internal class OfferInvalidExpiresAtException(DateTime expiresAt) : EcommerceException($"Expires at time: {expiresAt.ToString("dd-MM-yyyy")} is invalid")
    {
    }
}
