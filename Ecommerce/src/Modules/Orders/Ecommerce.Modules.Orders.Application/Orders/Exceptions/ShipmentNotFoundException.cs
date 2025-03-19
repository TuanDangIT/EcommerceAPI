using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    internal class ShipmentNotFoundException(Guid orderId, int shipmentId) : EcommerceException($"Shipment: {shipmentId} was not found in order: {orderId}.")
    {
    }
}
