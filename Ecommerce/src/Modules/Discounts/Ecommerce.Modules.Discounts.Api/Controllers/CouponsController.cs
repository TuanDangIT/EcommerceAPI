using Asp.Versioning;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
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
        [HttpGet("nominal-coupon")]
        public async  Task<ActionResult<ApiResponse<PagedResult<NominalCouponBrowseDto>>>> BrowseNominalCoupons([FromQuery] SieveModel model, CancellationToken cancellationToken)
            => PagedResult(await _couponService.BrowseNominalCouponsAsync(model, cancellationToken));
        [HttpGet("percentage-coupon")]
        public async Task<ActionResult<ApiResponse<PagedResult<PercentageCouponBrowseDto>>>> BrowsePercentageCoupons([FromQuery] SieveModel model, CancellationToken cancellationToken)
        {
            var result = await _couponService.BrowsePercentageCouponsAsync(model, cancellationToken);
            return PagedResult(result);
        }
        [HttpPost("nominal-coupon")]
        public async Task<ActionResult> CreateNominalCoupon([FromBody]NominalCouponCreateDto dto)
        {
            await _couponService.CreateAsync(dto);
            return NoContent();
        }
        [HttpPost("percentage-coupon")]
        public async Task<ActionResult> CreatePercentageCoupon([FromBody] PercentageCouponCreateDto dto)
        {
            await _couponService.CreateAsync(dto);
            return NoContent();
        }
        [HttpPut("{stripeCouponId}")]
        public async Task<ActionResult> UpdateCouponName([FromRoute]int couponId, [FromBody]CouponUpdateNameDto dto, CancellationToken cancellationToken = default)
        {
            await _couponService.UpdateNameAsync(couponId, dto, cancellationToken);
            return NoContent();
        }
        [HttpDelete("{stripeCouponId}")]
        public async Task<ActionResult> DeleteCoupon([FromRoute] int couponId, CancellationToken cancellationToken = default)
        {
            await _couponService.DeleteAsync(couponId, cancellationToken);
            return NoContent();
        }

    }
}
