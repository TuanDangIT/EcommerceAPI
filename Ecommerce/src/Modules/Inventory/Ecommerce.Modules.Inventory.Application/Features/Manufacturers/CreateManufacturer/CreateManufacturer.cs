using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Manufacturers.CreateManufacturer
{
    public sealed record class CreateManufacturer(string Name) : Shared.Abstractions.MediatR.ICommand;
}
