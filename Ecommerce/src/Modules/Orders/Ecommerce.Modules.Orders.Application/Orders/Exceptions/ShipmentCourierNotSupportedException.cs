﻿using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    internal class ShipmentCourierNotSupportedException(string courier) : EcommerceException($"Courier: {courier} is not supported.")
    {
    }
}
