using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.BrowseReviews;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Application.Shared.Sieve;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.EntityFrameworkCore;
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
    internal class BrowseReviewsHandler : IQueryHandler<BrowseReviews, PagedResult<ReviewDto>>
    {
        private readonly InventoryDbContext _dbContext;
        private readonly IOptions<SieveOptions> _sieveOptions;
        private readonly ISieveProcessor _sieveProcessor;

        public BrowseReviewsHandler(InventoryDbContext dbContext, IEnumerable<ISieveProcessor> sieveProcessors,
            IOptions<SieveOptions> sieveOptions)
        {
            _dbContext = dbContext;
            _sieveOptions = sieveOptions;
            _sieveProcessor = sieveProcessors.First(s => s.GetType() == typeof(InventoryModuleSieveProcessor));
        }
        public async Task<PagedResult<ReviewDto>> Handle(BrowseReviews request, CancellationToken cancellationToken)
        {
            if (request.Page is null)
            {
                throw new PaginationException();
            }
            var reviews = _dbContext.Reviews
               .Where(r => r.AuctionId == request.AuctionId)
               .AsNoTracking()
               .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(request, reviews)
                .Select(r => r.AsDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(request, reviews, applyPagination: false)
                .CountAsync(cancellationToken);
            int pageSize = _sieveOptions.Value.DefaultPageSize;
            if (request.PageSize is not null)
            {
                pageSize = request.PageSize.Value;
            }
            var pagedResult = new PagedResult<ReviewDto>(dtos, totalCount, pageSize, request.Page.Value);
            return pagedResult;
        }
    }
}
