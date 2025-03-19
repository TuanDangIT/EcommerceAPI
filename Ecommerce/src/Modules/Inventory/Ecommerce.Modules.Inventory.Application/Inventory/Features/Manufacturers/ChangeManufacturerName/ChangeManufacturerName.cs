using Ecommerce.Shared.Abstractions.MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.ChangeManufacturerName
{
    public sealed record class ChangeManufacturerName(string Name) : ICommand
    {
        [SwaggerIgnore]
        public Guid ManufaturerId { get; init; }
    }
}
