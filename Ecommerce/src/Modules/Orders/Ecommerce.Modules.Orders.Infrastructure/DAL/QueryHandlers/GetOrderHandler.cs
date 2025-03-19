using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.GetOrder;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class GetOrderHandler : IQueryHandler<GetOrder, OrderDetailsDto?>
    {
        private readonly OrdersDbContext _dbContext;

        public GetOrderHandler(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<OrderDetailsDto?> Handle(GetOrder request, CancellationToken cancellationToken)
            => await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Products)
                .Include(o => o.Shipments)
                .Include(o => o.Customer)
                .Include(o => o.Return)
                .ThenInclude(r => r!.Products)
                .Include(o => o.Complaints)
                .Where(o => o.Id == request.OrderId)
                .Select(o => o.AsDetailsDto())
                .FirstOrDefaultAsync(cancellationToken);
    }
}
