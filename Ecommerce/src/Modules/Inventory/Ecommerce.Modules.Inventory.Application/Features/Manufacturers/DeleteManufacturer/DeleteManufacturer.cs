using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteManufacturer
{
    public sealed record class DeleteManufacturer(Guid ManufacturerID) : Shared.Abstractions.MediatR.ICommand;
}
