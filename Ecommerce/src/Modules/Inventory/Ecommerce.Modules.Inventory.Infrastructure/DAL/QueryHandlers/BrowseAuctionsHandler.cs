using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.BrowseAuctions;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.QueryHandlers
{
    internal sealed class BrowseAuctionsHandler : IQueryHandler<BrowseAuctions, PagedResult<AuctionBrowseDto>>
    {
        private readonly InventoryDbContext _dbContext;
        private readonly ISieveProcessor _sieveProcessor;

        public BrowseAuctionsHandler(InventoryDbContext dbContext, ISieveProcessor sieveProcessor)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
        }
        public async Task<PagedResult<AuctionBrowseDto>> Handle(BrowseAuctions request, CancellationToken cancellationToken)
        {
            var auctions = _dbContext.Auctions
                .Include(a => a.Reviews)
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(request, auctions)
                .Select(a => a.AsBrowseDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(request, auctions, applyPagination: false)
                .CountAsync();
            if (request.PageSize is null || request.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<AuctionBrowseDto>(dtos, totalCount, request.PageSize.Value, request.Page.Value);
            return pagedResult;
        }
    }
}
