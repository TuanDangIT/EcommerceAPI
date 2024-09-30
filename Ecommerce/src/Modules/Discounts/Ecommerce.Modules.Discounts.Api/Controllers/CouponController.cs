using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Api.Controllers
{
    internal class CouponController : BaseController
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
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
