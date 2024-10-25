using Asp.Versioning;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination;
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
    [ApiVersion(1)]
    internal class CouponController : BaseController
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        [HttpGet("nominal-coupon")]
        public async  Task<ActionResult<ApiResponse<PagedResult<NominalCouponBrowseDto>>>> BrowseNominalCoupons([FromQuery]SieveModel model)
            => PagedResult(await _couponService.BrowseNominalCouponsAsync(model));
        [HttpGet("percentage-coupon")]
        public async Task<ActionResult<ApiResponse<PagedResult<PercentageCouponBrowseDto>>>> BrowsePercentageCoupons([FromQuery] SieveModel model)
        {
            var result = await _couponService.BrowsePercentageCouponsAsync(model);
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
        public async Task<ActionResult> UpdateCouponName([FromRoute]string stripeCouponId, [FromBody]CouponUpdateNameDto dto)
        {
            await _couponService.UpdateNameAsync(stripeCouponId, dto);
            return NoContent();
        }
        [HttpDelete("{stripeCouponId}")]
        public async Task<ActionResult> DeleteCoupon([FromRoute] string stripeCouponId)
        {
            await _couponService.DeleteAsync(stripeCouponId);
            return NoContent();
        }

    }
}
