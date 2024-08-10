using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Manufacturers.ChangeManufacturerName
{
    public sealed record class ChangeManufacturerName(Guid ManufaturerId, string Name) : Shared.Abstractions.MediatR.ICommand;
}
