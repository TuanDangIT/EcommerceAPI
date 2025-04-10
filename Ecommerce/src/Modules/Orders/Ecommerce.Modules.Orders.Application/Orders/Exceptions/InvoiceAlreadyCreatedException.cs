﻿using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    internal class InvoiceAlreadyCreatedException(Guid orderId) : EcommerceException($"Order: {orderId} invoice was already created. An order can have only one invoice.")
    {
    }
}
