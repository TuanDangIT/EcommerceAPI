using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.BrowseComplaints;
using Ecommerce.Modules.Orders.Application.Invoices.DTO;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
//using Ecommerce.Modules.Orders.Infrastructure.DAL.Services;
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
    internal class BrowseComplaintsHandler : IQueryHandler<BrowseComplaints, CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>>
    {
        private readonly OrdersDbContext _dbContext;
        private readonly IFilterService _filterService;

        public BrowseComplaintsHandler(OrdersDbContext dbContext, IFilterService filterService)
        {
            _dbContext = dbContext;
            _filterService = filterService;
        }
        public async Task<CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>> Handle(BrowseComplaints request, CancellationToken cancellationToken)
        {
            var complaintsAsQueryable = _dbContext.Complaints
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();
            if (request.Filters is not null && request.Filters.Count != 0)
            {
                foreach (var filter in request.Filters)
                {
                    complaintsAsQueryable = _filterService.ApplyFilter(complaintsAsQueryable, filter.Key, filter.Value);
                }
            }
            int takeAmount = request.PageSize + 1;
            if (request.CursorDto is not null)
            {
                if (request.IsNextPage is true)
                {
                    complaintsAsQueryable = complaintsAsQueryable.Where(c => c.CreatedAt <= request.CursorDto.CursorCreatedAt && c.Id != request.CursorDto.CursorId);
                }
                else
                {
                    complaintsAsQueryable = complaintsAsQueryable.Where(c => c.CreatedAt >= request.CursorDto.CursorCreatedAt && c.Id != request.CursorDto.CursorId);
                    takeAmount = request.PageSize;
                }
            }
            complaintsAsQueryable = complaintsAsQueryable.Take(takeAmount);
            if (request.IsNextPage is false && request.CursorDto is not null)
            {
                complaintsAsQueryable = complaintsAsQueryable.Reverse();
            }
            var complaints = await complaintsAsQueryable
                .Include(i => i.Order)
                .ThenInclude(o => o.Customer)
                .Select(o => o.AsBrowseDto())
                .AsNoTracking()
                .ToListAsync();
            bool isFirstPage = request.CursorDto is null
                || (request.CursorDto is not null && complaints.First().Id == _dbContext.Complaints.OrderByDescending(i => i.Id).AsNoTracking().First().Id);
            bool hasNextPage = complaints.Count > request.PageSize
                || (request.CursorDto is not null && request.IsNextPage == false);
            if (complaints.Count > request.PageSize)
            {
                complaints.RemoveAt(complaints.Count - 1);
            }
            ComplaintCursorDto nextCursor = hasNextPage ?
                new ComplaintCursorDto()
                {
                    CursorId = complaints.Last().Id,
                    CursorCreatedAt = complaints.Last().CreatedAt
                }
                : new();
            ComplaintCursorDto previousCursor = complaints.Count > 0 ?
                new ComplaintCursorDto()
                {
                    CursorId = complaints.First().Id,
                    CursorCreatedAt = complaints.First().CreatedAt
                }
                : new();
            return new CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>(complaints, nextCursor, previousCursor, isFirstPage);
        }
    }
}
