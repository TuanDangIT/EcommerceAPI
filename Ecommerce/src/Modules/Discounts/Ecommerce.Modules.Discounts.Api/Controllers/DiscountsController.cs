using Asp.Versioning;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Api.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class DiscountsController : BaseController
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }
        [HttpGet("coupons/{couponId:int}")]
        public async Task<ActionResult<ApiResponse<PagedResult<DiscountBrowseDto>>>> BrowseDiscounts([FromRoute]int couponId, [FromQuery]SieveModel model, 
            CancellationToken cancellationToken)
            => PagedResult(await _discountService.BrowseDiscountsAsync(couponId, model, cancellationToken));
        [HttpPost("coupons/{couponId:int}")]
        public async Task<ActionResult> CreateDiscount([FromRoute] int couponId, [FromBody] DiscountCreateDto dto, CancellationToken cancellationToken = default)
        {
            await _discountService.CreateAsync(couponId, dto, cancellationToken);
            return NoContent();
        }
        [HttpPut("{discountId:int}/activate")]
        public async Task<ActionResult> ActivateDiscount([FromRoute] int discountId, CancellationToken cancellationToken = default)
        {
            await _discountService.ActivateAsync(discountId, cancellationToken);
            return NoContent();
        }
        [HttpPut("{discountId:int}/deactivate")]
        public async Task<ActionResult> DeactivateDiscount([FromRoute] int discountId, CancellationToken cancellationToken = default)
        {
            await _discountService.DeactivateAsync(discountId, cancellationToken);
            return NoContent();
        }
    }
}
