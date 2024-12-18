using Asp.Versioning;
using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.BrowseAuctions;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.AddReview;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.BrowseReviews;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.DeleteReview;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.EditReview;
using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    [ApiVersion(1)]
    internal class ReviewController : BaseController
    {
        public ReviewController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("{auctionId:guid}")]
        public async Task<ActionResult<ApiResponse<PagedResult<ReviewDto>>>> GetReviews([FromQuery]BrowseReviews query, [FromRoute] Guid auctionId,
            CancellationToken cancellationToken = default)
        {
            query.AuctionId = auctionId;
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<ReviewDto>>(HttpStatusCode.OK, result));
        }
        [HttpPost("{auctionId:guid}")]
        public async Task<ActionResult> AddReview([FromRoute] Guid auctionId, [FromForm]AddReview command, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command with { AuctionId = auctionId }, cancellationToken);
            return NoContent();
        }
        [HttpDelete("{auctionId:guid}/{reviewId:guid}")]
        public async Task<ActionResult> DeleteReview([FromRoute]Guid auctionId, [FromRoute] Guid reviewId, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteReview(auctionId, reviewId), cancellationToken);
            return NoContent();
        }
        [HttpPut("{auctionId:guid}/{reviewId:guid}")]
        public async Task<ActionResult> DeleteReview([FromRoute] Guid auctionId, [FromRoute] Guid reviewId, [FromBody]EditReview command, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command with { AuctionId = auctionId, ReviewId = reviewId }, cancellationToken);
            return NoContent();
        }
    }
}
