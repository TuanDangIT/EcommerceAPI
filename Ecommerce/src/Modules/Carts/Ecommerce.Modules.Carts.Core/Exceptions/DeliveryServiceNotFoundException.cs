using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class DeliveryServiceNotFoundException(int deliveryServiceId) : EcommerceException($"Delivery service: {deliveryServiceId} was not found.")
    {
    }
}
