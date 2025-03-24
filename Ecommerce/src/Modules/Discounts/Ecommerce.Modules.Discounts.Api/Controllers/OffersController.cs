using Asp.Versioning;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Api.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class OffersController : BaseController
    {
        private readonly IOfferService _offerService;

        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [SwaggerOperation("Gets offset paginated offers.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for offers.", typeof(ApiResponse<PagedResult<DiscountBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [Authorize(Roles = "Admin, Manager, Employee, Customer")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<OfferBrowseDto>>>> BrowseOffers([FromQuery]SieveModel model, CancellationToken cancellationToken)
            => PagedResult(await _offerService.BrowseAsync(model, cancellationToken));

        [SwaggerOperation("Get a specific offer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a specific offer by id.", typeof(ApiResponse<OfferDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Offer was not found")]
        [Authorize(Roles = "Admin, Manager, Employee, Customer")]
        [HttpGet("{offerId:int}")]
        public async Task<ActionResult<ApiResponse<OfferDetailsDto>>> GetOffer([FromRoute] int offerId, CancellationToken cancellationToken)
            => OkOrNotFound(await _offerService.GetAsync(offerId, cancellationToken), nameof(Offer));

        [SwaggerOperation("Accepts an offer", "Accept an offer for a user to able to use it in a cart.")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{offerId:int}/accept")]
        public async Task<ActionResult> AcceptOffer([FromRoute]int offerId, CancellationToken cancellationToken)
        {
            await _offerService.AcceptAsync(offerId, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Rejects an offer", "Accept an offer for a user to able to use it in a cart.")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{offerId:int}/reject")]
        public async Task<ActionResult> RejectOffer([FromRoute]int offerId, CancellationToken cancellationToken)
        {
            await _offerService.RejectAsync(offerId, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Deletes an offer")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin, Manager, Employee, Customer")]
        [HttpDelete("{offerId:int}")]
        public async Task<ActionResult> DeleteOffer([FromRoute]int offerId, CancellationToken cancellationToken)
        {
            await _offerService.DeleteAsync(offerId, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Deletes an offer")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin, Manager, Employee, Customer")]
        [HttpDelete]
        public async Task<ActionResult> DeleteOffers([FromBody] IEnumerable<int> offerIds, CancellationToken cancellationToken)
        {
            await _offerService.DeleteManyAsync(offerIds, cancellationToken);
            return NoContent();
        }
    }
}
