using Ecommerce.Modules.Products.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewBrowseDto>> BrowseReviewsForProductAsync(Guid productId);
        Task<int> DeleteReviewAsync(Guid reviewId);
        Task<int> UpdateReviewForProductAsync(ReviewUpdateDto updateReviewDto, Guid productId);
        Task<int> AddReviewForProduct(ReviewAddDto reviewAddDto, Guid productId);
    }
}
