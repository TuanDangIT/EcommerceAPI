using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Api.Controllers
{
    internal class CartController : BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<CartDto>>> GetCart([FromRoute]Guid id)
        {
            var cart = await _cartService.GetAsync(id);
            return Ok(new ApiResponse<CartDto>(HttpStatusCode.OK, "success", cart));
        }
    }
}
