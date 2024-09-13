using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders
{
    public sealed record class BrowseOrders(CursorDto CursorDto, bool? IsNextPage, int PageSize) : IQuery<CursorPagedResult<OrderBrowseDto, CursorDto>>;
}
