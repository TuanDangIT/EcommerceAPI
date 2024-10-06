using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Api.Controllers
{
    internal class OfferController : BaseController
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<OfferBrowseDto>>>> BrowseOffers([FromQuery]SieveModel model)
        {
            var result = await _offerService.BrowseAsync(model);
            return PagedResult(result);
        }
        [HttpGet("{offerId:int}")]
        public async Task<ActionResult<ApiResponse<OfferDetailsDto>>> GetOffer([FromRoute] int offerId)
            => OkOrNotFound(await _offerService.GetAsync(offerId), nameof(Offer));
        [HttpPut("{offerId:int}/accept")]
        public async Task<ActionResult> AcceptOffer([FromRoute]int offerId)
        {
            await _offerService.AcceptAsync(offerId);
            return NoContent();
        }
        [HttpPut("{offerId:int}/reject")]
        public async Task<ActionResult> RejectOffer([FromRoute]int offerId)
        {
            await _offerService.RejectAsync(offerId);
            return NoContent();
        }
    }
}
