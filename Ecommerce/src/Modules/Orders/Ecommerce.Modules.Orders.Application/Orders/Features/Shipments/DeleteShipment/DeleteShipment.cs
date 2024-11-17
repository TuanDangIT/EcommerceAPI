using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.DeleteShipment
{
    public sealed record class DeleteShipment(Guid OrderId, int ShipmentId) : ICommand;
}
