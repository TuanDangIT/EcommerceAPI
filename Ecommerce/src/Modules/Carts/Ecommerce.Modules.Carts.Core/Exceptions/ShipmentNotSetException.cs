﻿using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Exceptions
{
    internal class ShipmentNotSetException : EcommerceException
    {
        public ShipmentNotSetException() : base("Shipment was not set.")
        {
        }
    }
}
