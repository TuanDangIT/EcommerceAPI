using Ecommerce.Modules.Orders.Application.Invoices.DTO;
using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.BrowseReturns;
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
    internal class BrowseReturnsHandler : IQueryHandler<BrowseReturns, CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>>
    {
        private readonly OrdersDbContext _dbContext;

        public BrowseReturnsHandler(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>> Handle(BrowseReturns request, CancellationToken cancellationToken)
        {
            var returnsAsQueryable = _dbContext.Returns.OrderBy(r => r.CreatedAt).AsQueryable();
            int takeAmount = request.PageSize + 1;
            if (request.CursorDto is not null)
            {
                if (request.IsNextPage is true)
                {
                    returnsAsQueryable = returnsAsQueryable.Where(r => r.CreatedAt >= request.CursorDto.CursorCreatedAt && r.Id != request.CursorDto.CursorId);
                }
                else
                {
                    returnsAsQueryable = returnsAsQueryable.Where(r => r.CreatedAt <= request.CursorDto.CursorCreatedAt && r.Id != request.CursorDto.CursorId);
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
                || (request.CursorDto is not null && returns.First().Id == _dbContext.Returns.OrderBy(r => r.Id).AsNoTracking().First().Id);
            bool hasNextPage = returns.Count > request.PageSize
                || (request.CursorDto is not null && request.IsNextPage == false);
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
