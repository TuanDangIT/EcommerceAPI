using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.BrowseOrders;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class BrowseOrdersHandler : IQueryHandler<BrowseOrders, IEnumerable<OrderBrowseDto>>
    {
        public Task<IEnumerable<OrderBrowseDto>> Handle(BrowseOrders request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
