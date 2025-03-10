using Asp.Versioning;
using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.BrowseAuctions;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.AddReview;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.BrowseReviews;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.DeleteReview;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.EditReview;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    [Route("api/v{v:apiVersion}/" + InventoryModule.BasePath + "/auctions/{auctionId:guid}/[controller]")]
    [Authorize(Roles = "Admin, Manager, Employee, Customer")]
    [ApiVersion(1)]
    internal class ReviewController : BaseController
    {
        public ReviewController(IMediator mediator) : base(mediator)
        {
        }

        [SwaggerOperation("Gets offset paginated reviews for auction")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for reviews for specified auction by id.", typeof(ApiResponse<PagedResult<ReviewDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ReviewDto>>>> GetReviews([FromQuery]BrowseReviews query, [FromRoute] Guid auctionId,
            CancellationToken cancellationToken = default)
        {
            query.AuctionId = auctionId;
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<ReviewDto>>(HttpStatusCode.OK, result));
        }

        [SwaggerOperation("Creates review for a specified auction")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> AddReview([FromRoute] Guid auctionId, [FromForm]AddReview command, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command with { AuctionId = auctionId }, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Deletes a review")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete("{reviewId:guid}")]
        public async Task<ActionResult> DeleteReview([FromRoute]Guid auctionId, [FromRoute] Guid reviewId, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteReview(auctionId, reviewId), cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Updates a review")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{reviewId:guid}")]
        public async Task<ActionResult> EditReview([FromRoute] Guid auctionId, [FromRoute] Guid reviewId, [FromBody]EditReview command, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command with { AuctionId = auctionId, ReviewId = reviewId }, cancellationToken);
            return NoContent();
        }
    }
}
