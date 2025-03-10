using Asp.Versioning;
using Ecommerce.Modules.Discounts.Core.DTO;
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
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Api.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class CouponsController : BaseController
    {
        private readonly ICouponService _couponService;

        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [SwaggerOperation("Gets offset paginated nominal coupons")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for nominal coupons.", typeof(ApiResponse<PagedResult<NominalCouponBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet("nominal-coupons")]
        public async  Task<ActionResult<ApiResponse<PagedResult<NominalCouponBrowseDto>>>> BrowseNominalCoupons([FromQuery] SieveModel model, CancellationToken cancellationToken)
            => PagedResult(await _couponService.BrowseNominalCouponsAsync(model, cancellationToken));

        [SwaggerOperation("Gets offset paginated percentage coupons")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for percentage coupons.", typeof(ApiResponse<PagedResult<PercentageCouponBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet("percentage-coupons")]
        public async Task<ActionResult<ApiResponse<PagedResult<PercentageCouponBrowseDto>>>> BrowsePercentageCoupons([FromQuery] SieveModel model, CancellationToken cancellationToken)
        {
            var result = await _couponService.BrowsePercentageCouponsAsync(model, cancellationToken);
            return PagedResult(result);
        }

        [SwaggerOperation("Creates a nominal coupon")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpPost("nominal-coupons")]
        public async Task<ActionResult> CreateNominalCoupon([FromBody]NominalCouponCreateDto dto)
        {
            await _couponService.CreateAsync(dto);
            return NoContent();
        }

        [SwaggerOperation("Creates a percentage coupon")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpPost("percentage-coupons")]
        public async Task<ActionResult> CreatePercentageCoupon([FromBody] PercentageCouponCreateDto dto)
        {
            await _couponService.CreateAsync(dto);
            return NoContent();
        }

        [SwaggerOperation("Updates a coupon's name")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{couponId:int}")]
        public async Task<ActionResult> UpdateCouponName([FromRoute]int couponId, [FromBody]CouponUpdateNameDto dto, CancellationToken cancellationToken = default)
        {
            await _couponService.UpdateNameAsync(couponId, dto, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Deletes a coupon")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpDelete("{couponId:int}")]
        public async Task<ActionResult> DeleteCoupon([FromRoute] int couponId, CancellationToken cancellationToken = default)
        {
            await _couponService.DeleteAsync(couponId, cancellationToken);
            return NoContent();
        }

    }
}
