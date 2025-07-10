using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class DeliveryServiceNotActiveException(int id) : EcommerceException($"Shipment delivery service with id {id} is unavailable.")
    {
    }
}
