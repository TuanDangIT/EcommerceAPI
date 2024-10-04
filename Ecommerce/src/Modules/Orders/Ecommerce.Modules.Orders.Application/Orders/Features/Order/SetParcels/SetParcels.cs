using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Domain.Shipping.Entities;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.SetParcels
{
    public sealed record class SetParcels(Guid OrderId, IEnumerable<ParcelDto> Parcels) : ICommand;
}
