﻿using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Events
{
    public sealed record class ProductQuantityChanged(Guid ProductId, int Quantity) : IEvent;
}
