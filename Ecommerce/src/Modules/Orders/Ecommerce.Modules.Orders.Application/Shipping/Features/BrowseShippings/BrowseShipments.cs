using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Shipping.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Shipping.Features.BrowseShippings
{
    public sealed record class BrowseShipments(ShipmentCursorDto? CursorDto, bool? IsNextPage, int PageSize) : IQuery<CursorPagedResult<ShipmentBrowseDto, ShipmentCursorDto>>;
}
