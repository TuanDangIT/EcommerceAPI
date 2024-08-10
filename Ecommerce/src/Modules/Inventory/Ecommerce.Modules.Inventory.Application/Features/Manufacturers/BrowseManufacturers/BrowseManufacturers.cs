﻿using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Manufacturers.BrowseManufacturers
{
    public sealed record class BrowseManufacturers : IQuery<IEnumerable<ManufacturerDto>>;
}
