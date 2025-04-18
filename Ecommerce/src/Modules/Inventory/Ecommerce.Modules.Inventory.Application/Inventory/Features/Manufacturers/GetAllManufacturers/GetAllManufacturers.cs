﻿using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.GetAllManufacturers
{
    public sealed record class GetAllManufacturers : IQuery<IEnumerable<ManufacturerOptionDto>>;
}
