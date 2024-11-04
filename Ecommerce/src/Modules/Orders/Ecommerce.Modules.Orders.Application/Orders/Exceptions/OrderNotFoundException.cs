using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    internal sealed class OrderNotFoundException : EcommerceException
    {
        public OrderNotFoundException(Guid orderId) : base($"Order: {orderId} was not found.")
        {
        }
        public OrderNotFoundException(string trackingNumber) : base($"Order: {trackingNumber} was not found.")
        {
            
        }
    }
}
