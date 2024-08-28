using Ecommerce.Modules.Auctions.Core.DTO;
using Ecommerce.Modules.Auctions.Core.Entities;
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
    internal class AuctionController : BaseController
    {
        private readonly IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<AuctionBrowseDto>>>> BrowseAuctions([FromBody]SieveModel sieveModel)
            => Ok(new ApiResponse<PagedResult<AuctionBrowseDto>>(HttpStatusCode.OK, "success", await _auctionService.BrowseAsync(sieveModel)));
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<AuctionDetailsDto>>> GetAuction([FromRoute] Guid id)
            => OkOrNotFound<AuctionDetailsDto, Auction>(await _auctionService.GetAsync(id));
    }
}
