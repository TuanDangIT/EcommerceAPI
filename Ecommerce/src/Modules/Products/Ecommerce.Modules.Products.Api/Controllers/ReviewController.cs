using Ecommerce.Modules.Products.Core.DTO;
using Ecommerce.Modules.Products.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Api.Controllers
{
    [Route("api/" + ProductsModule.BasePath + "/[controller]" + "{productId:guid}")]
    internal class ReviewController : BaseController
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet()]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReviewBrowseDto>>>> GetReviews([FromRoute] Guid productId)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public async Task<ActionResult> AddReview([FromRoute]Guid productId, [FromForm]ReviewAddDto reviewAddDto)
        {
            await _reviewService.AddReviewForProduct(reviewAddDto, productId);
            return NoContent();
        }
        [HttpDelete("{reviewId:guid}")]
        public async Task<ActionResult> DeleteReview([FromRoute]Guid reviewId)
        {
            await _reviewService.DeleteReviewAsync(reviewId);
            return NoContent();
        }
        
    }
}
