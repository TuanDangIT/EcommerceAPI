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

        public ReviewService(IProductDbContext dbContext)
        {
            _dbContext = dbContext;
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
