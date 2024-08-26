using Ecommerce.Modules.Products.Core.DAL;
using Ecommerce.Modules.Products.Core.DTO;
using Ecommerce.Modules.Products.Core.Entities;
using Ecommerce.Modules.Products.Core.Exceptions;
using Ecommerce.Modules.Products.Core.Services.Mappings;
using Microsoft.EntityFrameworkCore;
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
        private readonly TimeProvider _timeProvider;

        public ReviewService(IProductDbContext dbContext, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _timeProvider = timeProvider;
        }

        public async Task<int> AddReviewForProduct(ReviewAddDto reviewAddDto, Guid productId)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == productId);
            if(product is null)
            {
                throw new ProductNotFoundException(productId);
            }
            //nie koniec, jeszcze Username
            product.AddReview(new Review()
            {
                Id = Guid.NewGuid(),
                Text = reviewAddDto.Text,
                Grade = reviewAddDto.Grade,
                CreatedAt = _timeProvider.GetUtcNow().UtcDateTime
            });
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<ReviewBrowseDto>> BrowseReviewsForProductAsync(Guid productId)
            => await _dbContext.Reviews.Where(r => r.ProductId  == productId).Select(r => r.AsBrowseDto()).ToListAsync();

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
