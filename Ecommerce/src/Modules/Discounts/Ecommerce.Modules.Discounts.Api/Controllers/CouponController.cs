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
    internal class CouponController : BaseController
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
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
        public async Task<ActionResult> UpdateCouponName([FromRoute]string stripeCouponId, [FromBody]CouponUpdateNameDto dto, CancellationToken cancellationToken = default)
        {
            await _couponService.UpdateNameAsync(stripeCouponId, dto, cancellationToken);
            return NoContent();
        }
        [HttpDelete("{stripeCouponId}")]
        public async Task<ActionResult> DeleteCoupon([FromRoute] string stripeCouponId, CancellationToken cancellationToken = default)
        {
            await _couponService.DeleteAsync(stripeCouponId, cancellationToken);
            return NoContent();
        }

    }
}
