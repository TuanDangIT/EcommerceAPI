using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.BrowseReturns
{
    public sealed record class BrowseReturns : IQuery<CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>>;
}
