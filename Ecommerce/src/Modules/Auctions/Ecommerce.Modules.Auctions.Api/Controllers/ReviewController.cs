using Ecommerce.Modules.Auctions.Core.DTO;
using Ecommerce.Modules.Auctions.Core.Services;
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

namespace Ecommerce.Modules.Auctions.Api.Controllers
{
    [Route("api/" + AuctionsModule.BasePath + "/[controller]" + "{auctionId:guid}")]
    internal class ReviewController : BaseController
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet()]
        public async Task<ActionResult<ApiResponse<PagedResult<ReviewBrowseDto>>>> GetReviews([FromBody] SieveModel sieveModel, [FromRoute] Guid auctionId)
            => Ok(new ApiResponse<PagedResult<ReviewBrowseDto>>(HttpStatusCode.OK, "success", await _reviewService.BrowseAsync(sieveModel, auctionId)));
        [HttpPost]
        public async Task<ActionResult> AddReview([FromRoute]Guid auctionId, [FromForm]ReviewAddDto reviewAddDto)
        {
            await _reviewService.AddAsync(reviewAddDto, auctionId);
            return NoContent();
        }
        [HttpDelete("{reviewId:guid}")]
        public async Task<ActionResult> DeleteReview([FromRoute]Guid reviewId)
        {
            await _reviewService.DeleteAsync(reviewId);
            return NoContent();
        }
        
    }
}
