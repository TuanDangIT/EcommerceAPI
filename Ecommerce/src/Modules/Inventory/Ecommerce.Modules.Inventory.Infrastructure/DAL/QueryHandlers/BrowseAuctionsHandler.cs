using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.BrowseAuctions;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Application.Shared.Sieve;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sieve.Models;
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
        private readonly IOptions<SieveOptions> _sieveOptions;
        private readonly ISieveProcessor _sieveProcessor;

        public BrowseAuctionsHandler(InventoryDbContext dbContext, [FromKeyedServices("inventory-sieve-processor")]ISieveProcessor sieveProcessor,
            IOptions<SieveOptions> sieveOptions)
        {
            _dbContext = dbContext;
            _sieveOptions = sieveOptions;
            _sieveProcessor = sieveProcessor;
        }
        public async Task<PagedResult<AuctionBrowseDto>> Handle(BrowseAuctions request, CancellationToken cancellationToken)
        {
            if (request.Page is null)
            {
                throw new PaginationException();
            }
            var auctions = _dbContext.Auctions
                .Include(a => a.Reviews)
                .Where(a => a.IsSold == false)
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(request, auctions)
                .Select(a => a.AsBrowseDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(request, auctions, applyPagination: false)
                .Where(a => a.IsSold == false)
                .CountAsync(cancellationToken);
            int pageSize = _sieveOptions.Value.DefaultPageSize;
            if (request.PageSize is not null)
            {
                pageSize = request.PageSize.Value;
            }
            var pagedResult = new PagedResult<AuctionBrowseDto>(dtos, totalCount, pageSize, request.Page.Value);
            return pagedResult;
        }
    }
}
