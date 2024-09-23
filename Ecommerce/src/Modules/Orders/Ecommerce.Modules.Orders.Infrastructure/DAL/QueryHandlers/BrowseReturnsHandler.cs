using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.BrowseReturns;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class BrowseReturnsHandler : IQueryHandler<BrowseReturns, CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>>
    {
        public Task<CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>> Handle(BrowseReturns request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
