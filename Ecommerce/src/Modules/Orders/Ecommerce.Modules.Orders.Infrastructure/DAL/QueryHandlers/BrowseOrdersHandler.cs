using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class BrowseOrdersHandler : IQueryHandler<BrowseOrders, CursorPagedResult<OrderBrowseDto, CursorDto>>
    {
        private readonly OrdersDbContext _dbContext;

        public BrowseOrdersHandler(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CursorPagedResult<OrderBrowseDto, CursorDto>> Handle(BrowseOrders request, CancellationToken cancellationToken)
        {
            var ordersAsQueryable = _dbContext.Orders.OrderBy(o => o.OrderPlacedAt).AsQueryable();
            int takeAmount = request.PageSize + 1;
            if(request.CursorDto is not null)
            {
                if(request.IsNextPage is true)
                {
                    ordersAsQueryable = ordersAsQueryable.Where(o => o.OrderPlacedAt >= request.CursorDto.CursorOrderPlacedAt && o.Id != request.CursorDto.CursorId);
                }
                else
                {
                    ordersAsQueryable = ordersAsQueryable.Where(o => o.OrderPlacedAt <= request.CursorDto.CursorOrderPlacedAt && o.Id != request.CursorDto.CursorId);
                }
            }
            ordersAsQueryable = ordersAsQueryable.Take(takeAmount);
            if (request.IsNextPage is false && request.CursorDto is not null)
            {
                ordersAsQueryable = ordersAsQueryable.Reverse();
            }
            var orders = await ordersAsQueryable
                .Include(o => o.Products)
                .Select(o => o.AsBrowseDto())
                .AsNoTracking()
                .ToListAsync();
            bool isFirstPage = request.CursorDto is null
                || (request.CursorDto is not null && orders.First().Id == _dbContext.Orders.OrderBy(g => g.Id).AsNoTracking().First().Id);
            bool hasNextPage = orders.Count > request.PageSize 
                || (request.CursorDto is not null && request.IsNextPage == false);
            CursorDto nextCursor = hasNextPage ?
                new CursorDto()
                {
                    CursorId = orders.Last().Id,
                    CursorOrderPlacedAt = orders.Last().OrderPlacedAt
                }
                : new();
            CursorDto previousCursor = orders.Count > 0 ?
                new CursorDto()
                {
                    CursorId = orders.First().Id,
                    CursorOrderPlacedAt = orders.First().OrderPlacedAt
                }
                : new();
            return new CursorPagedResult<OrderBrowseDto, CursorDto>(orders, nextCursor, previousCursor, isFirstPage);

        }
    }
}
