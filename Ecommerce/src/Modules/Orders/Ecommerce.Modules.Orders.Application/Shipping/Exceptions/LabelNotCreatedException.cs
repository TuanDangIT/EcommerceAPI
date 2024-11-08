using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Shipping.Exceptions
{
    internal class LabelNotCreatedException(int shipmentId) : EcommerceException($"Label was not created for shipment: {shipmentId}")
    {
    }
}
