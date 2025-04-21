using Asp.Versioning;
using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Modules.Inventory.Application.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.BrowseAuctions;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.GetAuction;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.RequestOffer;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    [ApiVersion(1)]
    internal class AuctionsController : BaseController
    {
        public AuctionsController(IMediator mediator) : base(mediator)
        {
        }

        [SwaggerOperation("Gets offset paginated auctions")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for auctions.", typeof(ApiResponse<PagedResult<AuctionBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<AuctionBrowseDto>>>> BrowseAuctions([FromQuery]BrowseAuctions query, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<AuctionBrowseDto>>(HttpStatusCode.OK, result));
        }

        [SwaggerOperation("Get a specific auction")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a specific auction by id.", typeof(ApiResponse<AuctionDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Auction was not found")]
        [HttpGet("{auctionId:guid}")]
        public async Task<ActionResult<ApiResponse<AuctionDetailsDto>>> GetAuction([FromRoute] Guid auctionId, 
            CancellationToken cancellationToken)
            => OkOrNotFound<AuctionDetailsDto, Auction>(await _mediator.Send(new GetAuction(auctionId), cancellationToken), auctionId.ToString());

        [SwaggerOperation("Sends request offer")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [Authorize]
        [HttpPost("{auctionId:guid}/offers")]
        public async Task<ActionResult> RequestOffer([FromRoute]Guid auctionId, [FromForm]RequestOffer command, 
            CancellationToken cancellationToken)
        {
            command = command with { AuctionId = auctionId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

    }
}
