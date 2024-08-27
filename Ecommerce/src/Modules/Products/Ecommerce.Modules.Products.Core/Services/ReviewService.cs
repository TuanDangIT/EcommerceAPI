using Azure.Core;
using Ecommerce.Modules.Products.Core.DAL;
using Ecommerce.Modules.Products.Core.DTO;
using Ecommerce.Modules.Products.Core.Entities;
using Ecommerce.Modules.Products.Core.Exceptions;
using Ecommerce.Modules.Products.Core.Services.Mappings;
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

namespace Ecommerce.Modules.Products.Core.Services
{
    internal class ReviewService : IReviewService
    {
        private readonly IProductDbContext _dbContext;
        private readonly IIdentityContext _identityContext;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly TimeProvider _timeProvider;

        public ReviewService(IProductDbContext dbContext, IIdentityContext identityContext, ISieveProcessor sieveProcessor, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _identityContext = identityContext;
            _sieveProcessor = sieveProcessor;
            _timeProvider = timeProvider;
        }

        public async Task<int> AddReviewForProductAsync(ReviewAddDto reviewAddDto, Guid productId)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == productId);
            if(product is null)
            {
                throw new ProductNotFoundException(productId);
            }
            product.AddReview(new Review()
            {
                Id = Guid.NewGuid(),
                Text = reviewAddDto.Text,
                Grade = reviewAddDto.Grade,
                CreatedAt = _timeProvider.GetUtcNow().UtcDateTime,
                Username = _identityContext.IsAuthenticated ? _identityContext.Username : throw new UserIsNotAuthenticatedException()
            });
            return await _dbContext.SaveChangesAsync();
        }
        //public async Task<IEnumerable<ReviewBrowseDto>> BrowseReviewsForProductAsync(Guid productId)
        //    => await _dbContext.Reviews.Where(r => r.ProductId  == productId).OrderByDescending(r => r.CreatedAt).Select(r => r.AsBrowseDto()).ToListAsync();
        public async Task<PagedResult<ReviewBrowseDto>> BrowseReviewsForProductAsync(SieveModel sieveModel, Guid productId)
        {
            var reviews = _dbContext.Reviews
                .Where(r => r.ProductId == productId)
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

        public async Task<int> DeleteReviewAsync(Guid reviewId)
            => await _dbContext.Reviews.Where(r => r.Id == reviewId).ExecuteDeleteAsync();

        public async Task<int> UpdateReviewForProductAsync(ReviewUpdateDto updateReviewDto, Guid reviewId)
        {
            var review = await _dbContext.Reviews.SingleOrDefaultAsync(r => r.Id == reviewId);
            if(review is null)
            {
                throw new ReviewNotFoundException(reviewId);
            }
            review.Text = updateReviewDto.Text;
            review.Grade = updateReviewDto.Grade;
            return await _dbContext.SaveChangesAsync();
        }
    }
}
