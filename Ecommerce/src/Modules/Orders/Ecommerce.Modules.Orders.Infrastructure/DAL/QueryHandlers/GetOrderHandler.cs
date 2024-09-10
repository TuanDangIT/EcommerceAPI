using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.GetOrder;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class GetOrderHandler : IQueryHandler<GetOrder, OrderDetailsDto>
    {
        public GetOrderHandler()
        {
            
        }
        public Task<OrderDetailsDto> Handle(GetOrder request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
