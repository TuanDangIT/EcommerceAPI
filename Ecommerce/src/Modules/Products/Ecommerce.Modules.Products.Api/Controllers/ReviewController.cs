using Ecommerce.Modules.Products.Core.DTO;
using Ecommerce.Modules.Products.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public async Task<ActionResult<ApiResponse<PagedResult<ReviewBrowseDto>>>> GetReviews([FromBody] SieveModel sieveModel, [FromRoute] Guid productId)
            => Ok(new ApiResponse<PagedResult<ReviewBrowseDto>>(HttpStatusCode.OK, "success", await _reviewService.BrowseReviewsForProductAsync(sieveModel, productId)));
        [HttpPost]
        public async Task<ActionResult> AddReview([FromRoute]Guid productId, [FromForm]ReviewAddDto reviewAddDto)
        {
            await _reviewService.AddReviewForProductAsync(reviewAddDto, productId);
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
