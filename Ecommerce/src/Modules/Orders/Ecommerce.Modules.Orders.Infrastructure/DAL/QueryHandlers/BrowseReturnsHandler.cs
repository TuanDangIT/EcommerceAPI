using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.BrowseReturns;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class BrowseReturnsHandler : IQueryHandler<BrowseReturns, CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>>
    {
        private readonly OrdersDbContext _dbContext;
        private readonly IFilterService _filterService;

        public BrowseReturnsHandler(OrdersDbContext dbContext, IFilterService filterService)
        {
            _dbContext = dbContext;
            _filterService = filterService;
        }
        public async Task<CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>> Handle(BrowseReturns request, CancellationToken cancellationToken)
        {
            var returnsAsQueryable = _dbContext.Returns
                .OrderByDescending(r => r.CreatedAt)
                .ThenBy(r => r.Id)
                .AsQueryable();
            if (request.Filters is not null && request.Filters.Count != 0)
            {
                foreach (var filter in request.Filters)
                {
                    returnsAsQueryable = _filterService.ApplyFilter(returnsAsQueryable, filter.Key, filter.Value);
                }
            }
            int takeAmount = request.PageSize + 1;
            if (request.CursorDto is not null)
            {
                if (request.IsNextPage is true)
                {
                    returnsAsQueryable = returnsAsQueryable.Where(r => r.CreatedAt <= request.CursorDto.CursorCreatedAt && r.Id != request.CursorDto.CursorId);
                }
                else
                {
                    returnsAsQueryable = returnsAsQueryable.Where(r => r.CreatedAt >= request.CursorDto.CursorCreatedAt && r.Id != request.CursorDto.CursorId);
                    takeAmount = request.PageSize;
                }
            }
            returnsAsQueryable = returnsAsQueryable.Take(takeAmount);
            if (request.IsNextPage is false && request.CursorDto is not null)
            {
                returnsAsQueryable = returnsAsQueryable.Reverse();
            }
            var returns = await returnsAsQueryable
                .Include(r => r.Order)
                .Select(r => r.AsBrowseDto())
                .AsNoTracking()
                .ToListAsync();
            bool isFirstPage = request.CursorDto is null
                || (request.CursorDto is not null && returns.First().Id == _dbContext.Returns.OrderByDescending(r => r.CreatedAt)
                    .ThenBy(r => r.Id).AsNoTracking().First().Id);
            bool hasNextPage = returns.Count > request.PageSize
                || (request.CursorDto is not null && request.IsNextPage == false);
            if (returns.Count > request.PageSize)
            {
                returns.RemoveAt(returns.Count - 1);
            }
            ReturnCursorDto nextCursor = hasNextPage ?
                new ReturnCursorDto()
                {
                    CursorId = returns.Last().Id,
                    CursorCreatedAt = returns.Last().CreatedAt
                }
                : new();
            ReturnCursorDto previousCursor = returns.Count > 0 ?
                new ReturnCursorDto()
                {
                    CursorId = returns.First().Id,
                    CursorCreatedAt = returns.First().CreatedAt
                }
                : new();
            return new CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>(returns, nextCursor, previousCursor, isFirstPage);
        }
    }
}
