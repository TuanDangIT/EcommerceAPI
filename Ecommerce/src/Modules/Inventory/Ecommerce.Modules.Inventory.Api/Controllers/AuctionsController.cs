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
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<AuctionBrowseDto>>>> BrowseAuctions([FromQuery]BrowseAuctions query, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<AuctionBrowseDto>>(HttpStatusCode.OK, result));
        }
        [HttpGet("{auctionId:guid}")]
        public async Task<ActionResult<ApiResponse<AuctionDetailsDto>>> GetAuction([FromRoute] Guid auctionId, 
            CancellationToken cancellationToken = default)
            => OkOrNotFound<AuctionDetailsDto, Auction>(await _mediator.Send(new GetAuction(auctionId), cancellationToken));
        [Authorize]
        [HttpPost("{auctionId:guid}/offer")]
        public async Task<ActionResult> RequestOffer([FromRoute]Guid auctionId, [FromForm]RequestOffer command, 
            CancellationToken cancellationToken = default)
        {
            command = command with { AuctionId = auctionId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

    }
}
