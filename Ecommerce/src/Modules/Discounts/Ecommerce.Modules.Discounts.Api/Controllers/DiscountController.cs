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
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Api.Controllers
{
    internal class DiscountController : BaseController
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }
        [HttpPost("nominal-discount")]
        public async Task<ActionResult> CreateNominalDiscount([FromBody]NominalDiscountCreateDto dto)
        {
            await _discountService.CreateAsync(dto);
            return NoContent();
        }
        [HttpPost("percentage-discount")]
        public async Task<ActionResult> CreatePercentageDiscount([FromBody] PercentageDiscountCreateDto dto)
        {
            await _discountService.CreateAsync(dto);
            return NoContent();
        }
        [HttpGet("nominal-discount")]
        public async Task<ActionResult<ApiResponse<PagedResult<NominalDiscountBrowseDto>>>> BrowseNominalDiscounts([FromQuery] SieveModel model)
        {
            var result = await _discountService.BrowseNominalDiscountsAsync(model);
            return Ok(new ApiResponse<PagedResult<NominalDiscountBrowseDto>>(HttpStatusCode.OK, result));
        }
        [HttpGet("percentage-discount")]
        public async Task<ActionResult<ApiResponse<PagedResult<PercentageDiscountBrowseDto>>>> BrowsePercentageDiscounts([FromQuery] SieveModel model)
        {
            var result = await _discountService.BrowsePercentageDiscountsAsync(model);
            return Ok(new ApiResponse<PagedResult<PercentageDiscountBrowseDto>>(HttpStatusCode.OK, result));
        }
        [HttpDelete()]
        public async Task<ActionResult> DeleteDiscount([FromBody]string code)
        {
            await _discountService.DeleteAsync(code);
            return NoContent();
        }
    }
}
