using Asp.Versioning;
using Azure;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Stripe;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Api.Controllers
{
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/" + DiscountsModule.BasePath + "/coupons/{couponId:int}/[controller]")]
    internal class DiscountsController : BaseController
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [SwaggerOperation("Gets all discounts")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns all discounts for certain coupon.", typeof(ApiResponse<IEnumerable<DiscountBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<DiscountBrowseDto>>>> BrowseDiscounts([FromRoute]int couponId, CancellationToken cancellationToken)
            => new ApiResponse<IEnumerable<DiscountBrowseDto>>(HttpStatusCode.OK, await _discountService.BrowseDiscountsAsync(couponId, cancellationToken));

        [SwaggerOperation("Creates discount")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> CreateDiscount([FromRoute] int couponId, [FromBody] DiscountCreateDto dto, CancellationToken cancellationToken)
        {
            var discountId = await _discountService.CreateAsync(couponId, dto, cancellationToken);
            Response.Headers.Append("discount-id", discountId.ToString());
            return Created(discountId);
        }

        [SwaggerOperation("Activates a discount", "Activate a discount to able to use it in a cart.")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{discountId:int}/activate")]
        public async Task<ActionResult> ActivateDiscount([FromRoute] int couponId, [FromRoute] int discountId, CancellationToken cancellationToken)
        {
            await _discountService.ActivateAsync(couponId, discountId, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Deactivates a discount")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{discountId:int}/deactivate")]
        public async Task<ActionResult> DeactivateDiscount([FromRoute] int couponId, [FromRoute] int discountId, CancellationToken cancellationToken)
        {
            await _discountService.DeactivateAsync(couponId, discountId, cancellationToken);
            return NoContent();
        }
    }
}
