using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class ShipmentNotSupportedCountryException : EcommerceException
    {
        public ShipmentNotSupportedCountryException() : base("Only Poland is currently supported.")
        {
        }
    }
}
