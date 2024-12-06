using Asp.Versioning;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Api.Controllers
{
    [ApiVersion(1)]
    internal class OfferController : BaseController
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<OfferBrowseDto>>>> BrowseOffers([FromQuery]SieveModel model, CancellationToken cancellationToken)
            => PagedResult(await _offerService.BrowseAsync(model, cancellationToken));
        [HttpGet("{offerId:int}")]
        public async Task<ActionResult<ApiResponse<OfferDetailsDto>>> GetOffer([FromRoute] int offerId, CancellationToken cancellationToken = default)
            => OkOrNotFound(await _offerService.GetAsync(offerId, cancellationToken), nameof(Offer));
        [HttpPut("{offerId:int}/accept")]
        public async Task<ActionResult> AcceptOffer([FromRoute]int offerId, CancellationToken cancellationToken = default)
        {
            await _offerService.AcceptAsync(offerId, cancellationToken);
            return NoContent();
        }
        [HttpPut("{offerId:int}/reject")]
        public async Task<ActionResult> RejectOffer([FromRoute]int offerId, CancellationToken cancellationToken = default)
        {
            await _offerService.RejectAsync(offerId, cancellationToken);
            return NoContent();
        }
    }
}
