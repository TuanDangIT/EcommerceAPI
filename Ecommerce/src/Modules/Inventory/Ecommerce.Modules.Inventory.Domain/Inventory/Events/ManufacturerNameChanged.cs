﻿using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Events
{
    public record class ManufacturerNameChanged(string manufacturerName) : IEvent;
}
