using Azure.Core;
using Ecommerce.Modules.Auctions.Core.DAL;
using Ecommerce.Modules.Auctions.Core.DTO;
using Ecommerce.Modules.Auctions.Core.Entities;
using Ecommerce.Modules.Auctions.Core.Exceptions;
using Ecommerce.Modules.Auctions.Core.Services.Mappings;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Infrastructure.Pagination;
using MediatR;
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
    internal class ReviewService : IReviewService
    {
        private readonly IAuctionDbContext _dbContext;
        private readonly IContextService _contextService;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly TimeProvider _timeProvider;

        public ReviewService(IAuctionDbContext dbContext, IContextService contextService, ISieveProcessor sieveProcessor, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _sieveProcessor = sieveProcessor;
            _timeProvider = timeProvider;
        }

        public async Task<int> AddAsync(ReviewAddDto reviewAddDto, Guid auctionId)
        {
            var auction = await _dbContext.Auctions.SingleOrDefaultAsync(p => p.Id == auctionId);
            if (auction is null)
            {
                throw new AuctionNotFoundException(auctionId);
            }
            auction.AddReview(new Review(
                Guid.NewGuid(),
                _contextService.Identity is not null && _contextService.Identity.IsAuthenticated ? 
                _contextService.Identity.Username : 
                throw new UserIsNotAuthenticatedException(),
                reviewAddDto.Text,
                reviewAddDto.Grade,
                _timeProvider.GetUtcNow().UtcDateTime));
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<PagedResult<ReviewBrowseDto>> BrowseAsync(SieveModel sieveModel, Guid auctionId)
        {
            var reviews = _dbContext.Reviews
                .Where(r => r.AuctionId == auctionId)
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(sieveModel, reviews)
                .Select(r => r.AsBrowseDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(sieveModel, reviews, applyPagination: false)
                .CountAsync();
            if (sieveModel.PageSize is null || sieveModel.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<ReviewBrowseDto>(dtos, totalCount, sieveModel.PageSize.Value, sieveModel.Page.Value);
            return pagedResult;
        }

        public async Task<int> DeleteAsync(Guid reviewId)
            => await _dbContext.Reviews.Where(r => r.Id == reviewId).ExecuteDeleteAsync();

        public async Task<int> UpdateAsync(ReviewUpdateDto updateReviewDto, Guid reviewId)
        {
            var review = await _dbContext.Reviews.SingleOrDefaultAsync(r => r.Id == reviewId);
            if(review is null)
            {
                throw new ReviewNotFoundException(reviewId);
            }
            review.UpdateReview(updateReviewDto.Text, updateReviewDto.Grade, _timeProvider.GetUtcNow().UtcDateTime);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
