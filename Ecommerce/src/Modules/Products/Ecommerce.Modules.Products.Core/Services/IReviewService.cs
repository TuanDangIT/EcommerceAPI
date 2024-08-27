using Ecommerce.Modules.Products.Core.DTO;
using Ecommerce.Shared.Infrastructure.Pagination;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Services
{
    public interface IReviewService
    {
        Task<PagedResult<ReviewBrowseDto>> BrowseReviewsForProductAsync(SieveModel sieveModel, Guid productId);
        Task<int> DeleteReviewAsync(Guid reviewId);
        Task<int> UpdateReviewForProductAsync(ReviewUpdateDto updateReviewDto, Guid productId);
        Task<int> AddReviewForProductAsync(ReviewAddDto reviewAddDto, Guid productId);
    }
}
