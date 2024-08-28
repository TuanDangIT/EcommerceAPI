using Ecommerce.Modules.Auctions.Core.DAL;
using Ecommerce.Modules.Auctions.Core.DTO;
using Ecommerce.Modules.Auctions.Core.Entities;
using Ecommerce.Modules.Auctions.Core.Exceptions;
using Ecommerce.Modules.Auctions.Core.Services.Mappings;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.Services
{
    internal class AuctionService : IAuctionService
    {
        private readonly IAuctionDbContext _dbContext;
        private readonly ISieveProcessor _sieveProcessor;

        public AuctionService(IAuctionDbContext dbContext, ISieveProcessor sieveProcessor)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
        }
        public async Task<PagedResult<AuctionBrowseDto>> BrowseAsync(SieveModel sieveModel)
        {
            var auctions = _dbContext.Auctions
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(sieveModel, auctions)
                .Select(r => r.AsBrowseDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(sieveModel, auctions, applyPagination: false)
                .CountAsync();
            if (sieveModel.PageSize is null || sieveModel.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<AuctionBrowseDto>(dtos, totalCount, sieveModel.PageSize.Value, sieveModel.Page.Value);
            return pagedResult;
        }
        public async Task<AuctionDetailsDto?> GetAsync(Guid auctionId)
        {
            var auction = await _dbContext.Auctions
                .Include(p => p.Reviews)
                .SingleOrDefaultAsync(p => p.Id == auctionId);
            return auction?.AsDetailsDto();
        }
    }
}
