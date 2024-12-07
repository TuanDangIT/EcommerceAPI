using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.BrowseInvoices;
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
    internal class BrowseInvoicesHandler : IQueryHandler<BrowseInvoices, CursorPagedResult<InvoiceBrowseDto, InvoiceCursorDto>>
    {
        private readonly OrdersDbContext _dbContext;
        private readonly IFilterService _filterService;

        public BrowseInvoicesHandler(OrdersDbContext dbContext, IFilterService filterService)
        {
            _dbContext = dbContext;
            _filterService = filterService;
        }
        public async Task<CursorPagedResult<InvoiceBrowseDto, InvoiceCursorDto>> Handle(BrowseInvoices request, CancellationToken cancellationToken)
        {
            var invoicesAsQueryable = _dbContext.Invoices
                .OrderByDescending(i => i.CreatedAt)
                .ThenBy(i => i.Id)
                .AsQueryable();
            if (request.Filters is not null && request.Filters.Count != 0)
            {
                foreach (var filter in request.Filters)
                {
                    invoicesAsQueryable = _filterService.ApplyFilter(invoicesAsQueryable, filter.Key, filter.Value);
                }
            }
            int takeAmount = request.PageSize + 1;
            if (request.CursorDto is not null)
            {
                if (request.IsNextPage is true)
                {
                    invoicesAsQueryable = invoicesAsQueryable.Where(i => i.CreatedAt <= request.CursorDto.CursorCreatedAt && i.Id != request.CursorDto.CursorId);
                }
                else
                {
                    invoicesAsQueryable = invoicesAsQueryable.Where(i => i.CreatedAt >= request.CursorDto.CursorCreatedAt && i.Id != request.CursorDto.CursorId);
                    takeAmount = request.PageSize;
                }
            }
            invoicesAsQueryable = invoicesAsQueryable.Take(takeAmount);
            if (request.IsNextPage is false && request.CursorDto is not null)
            {
                invoicesAsQueryable = invoicesAsQueryable.Reverse();
            }
            var invoices = await invoicesAsQueryable
                .Include(i => i.Order)
                .ThenInclude(o => o.Customer)
                .Select(o => o.AsBrowseDto())
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            bool isFirstPage = request.CursorDto is null
                || (request.CursorDto is not null && invoices.First().Id == _dbContext.Invoices.OrderByDescending(i => i.CreatedAt)
                    .ThenBy(i => i.Id).AsNoTracking().First().Id);
            bool hasNextPage = invoices.Count > request.PageSize
                || (request.CursorDto is not null && request.IsNextPage == false);
            if (invoices.Count > request.PageSize)
            {
                invoices.RemoveAt(invoices.Count - 1);
            }
            InvoiceCursorDto nextCursor = hasNextPage ?
                new InvoiceCursorDto()
                {
                    CursorId = invoices.Last().Id,
                    CursorCreatedAt = invoices.Last().CreatedAt
                }
                : new();
            InvoiceCursorDto previousCursor = invoices.Count > 0 ?
                new InvoiceCursorDto()
                {
                    CursorId = invoices.First().Id,
                    CursorCreatedAt = invoices.First().CreatedAt
                }
                : new();
            return new CursorPagedResult<InvoiceBrowseDto, InvoiceCursorDto>(invoices, nextCursor, previousCursor, isFirstPage);
        }
    }
}
