using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.GetOrder;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.Contexts;
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
        private readonly IContextService _contextService;

        public GetOrderHandler(OrdersDbContext dbContext, IContextService contextService)
        {
            _dbContext = dbContext;
            _contextService = contextService;
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
                .Select(o => o.AsDetailsDto(_contextService.Identity == null || _contextService.Identity.Id == Guid.Empty || _contextService.Identity.Role == "Customer"))
                .FirstOrDefaultAsync(cancellationToken);
    }
}
