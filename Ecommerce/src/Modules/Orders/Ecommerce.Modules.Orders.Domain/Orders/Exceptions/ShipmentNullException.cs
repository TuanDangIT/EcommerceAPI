using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal class ShipmentNullException : EcommerceException
    {
        public string PropertyName;
        public ShipmentNullException(string propertyName) : base($"{propertyName} cannot be null.")
        {
            PropertyName = propertyName;
        }
    }
}
