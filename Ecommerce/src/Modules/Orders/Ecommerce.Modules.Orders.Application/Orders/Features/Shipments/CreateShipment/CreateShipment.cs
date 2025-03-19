using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects;
using Ecommerce.Shared.Abstractions.MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.CreateShipment
{
    public sealed record class CreateShipment(IEnumerable<ParcelDto> Parcels) : ICommand
    {
        [SwaggerIgnore]
        public Guid OrderId { get; init; }
    }
}
