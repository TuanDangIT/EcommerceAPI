using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.BrowseAuctions;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.GetAuction;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    internal class AuctionController : BaseController
    {
        public AuctionController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<AuctionBrowseDto>>>> BrowseAuctions([FromQuery]BrowseAuctions query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<PagedResult<AuctionBrowseDto>>(HttpStatusCode.OK, "success", result));
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<AuctionDetailsDto>>> GetAuction([FromRoute] Guid id)
            => OkOrNotFound<AuctionDetailsDto, Auction>(await _mediator.Send(new GetAuction(id)));

    }
}
