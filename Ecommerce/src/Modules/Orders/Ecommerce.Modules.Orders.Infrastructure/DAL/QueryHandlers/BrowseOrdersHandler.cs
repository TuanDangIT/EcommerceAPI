using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class BrowseOrdersHandler : IQueryHandler<BrowseOrders, CursorPagedResult<OrderBrowseDto, OrderCursorDto>>
    {
        private readonly OrdersDbContext _dbContext;
        private readonly IFilterService _filterService;

        public BrowseOrdersHandler(OrdersDbContext dbContext, IFilterService filterService)
        {
            _dbContext = dbContext;
            _filterService = filterService;
        }
        public async Task<CursorPagedResult<OrderBrowseDto, OrderCursorDto>> Handle(BrowseOrders request, CancellationToken cancellationToken)
        {
            var ordersAsQueryable = _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Shipments)
                .Include(o => o.Invoice)
                .OrderByDescending(o => o.CreatedAt)
                .ThenBy(o => o.Id)
                .AsQueryable();
            if (request.Filters?.Count != 0)
            {
                foreach (var filter in request.Filters!)
                {
                    ordersAsQueryable = _filterService.ApplyFilter(ordersAsQueryable, filter.Key, filter.Value);
                }
            }
            int takeAmount = request.PageSize + 1;
            if(request.CursorDto is not null)
            {
                if(request.IsNextPage is true)
                {
                    ordersAsQueryable = ordersAsQueryable.Where(o => o.CreatedAt <= request.CursorDto.CursorOrderPlacedAt && o.Id != request.CursorDto.CursorId);
                }
                else
                {
                    ordersAsQueryable = ordersAsQueryable.Where(o => o.CreatedAt >= request.CursorDto.CursorOrderPlacedAt && o.Id != request.CursorDto.CursorId);
                    takeAmount = request.PageSize;
                }
            }
            ordersAsQueryable = ordersAsQueryable.Take(takeAmount);
            if (request.IsNextPage is false && request.CursorDto is not null)
            {
                ordersAsQueryable = ordersAsQueryable.Reverse();
            }
            var orders = await ordersAsQueryable
                .Select(o => o.AsBrowseDto())
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            bool isFirstPage = request.CursorDto is null
                || (request.CursorDto is not null && orders.First().Id == _dbContext.Orders.OrderByDescending(o => o.CreatedAt)
                    .ThenBy(o => o.Id).AsNoTracking().First().Id);

            bool hasNextPage = orders.Count > request.PageSize 
                || (request.CursorDto is not null && request.IsNextPage == false);
            if (orders.Count > request.PageSize)
            {
                orders.RemoveAt(orders.Count - 1);
            }
            OrderCursorDto nextCursor = hasNextPage ?
                new OrderCursorDto()
                {
                    CursorId = orders.Last().Id,
                    CursorOrderPlacedAt = orders.Last().PlacedAt
                }
                : new();
            OrderCursorDto previousCursor = orders.Count > 0 ?
                new OrderCursorDto()
                {
                    CursorId = orders.First().Id,
                    CursorOrderPlacedAt = orders.First().PlacedAt
                }
                : new();

            return new CursorPagedResult<OrderBrowseDto, OrderCursorDto>(orders, nextCursor, previousCursor, isFirstPage);
        }
    }
}
