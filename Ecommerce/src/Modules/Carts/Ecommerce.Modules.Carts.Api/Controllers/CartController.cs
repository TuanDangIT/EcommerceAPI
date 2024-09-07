using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
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
        [HttpPost]
        public async Task<ActionResult> CreateCart()
        {
            var cartGuid = await _cartService.CreateAsync();
            //return CreatedAtAction(nameof(GetCart), new { cartGuid }, null);
            return NoContent();
        }
        [HttpGet("{cartId:guid}")]
        public async Task<ActionResult<ApiResponse<CartDto?>>> GetCart([FromRoute]Guid cartId)
            => OkOrNotFound<CartDto?>(await _cartService.GetAsync(cartId), "Cart");
        [HttpPost("{cartId:guid}")]
        public async Task<ActionResult> AddProduct([FromRoute]Guid cartId, [FromBody]CartAddProductDto dto)
        {
            await _cartService.AddProductAsync(cartId, dto.ProductId, dto.Quantity);
            return NoContent();
        }
        [HttpDelete("{cartId:guid}")]
        public async Task<ActionResult> RemoveProduct([FromRoute]Guid cartId, [FromBody]Guid productId)
        {
            await _cartService.RemoveProduct(cartId, productId);
            return NoContent();
        }
        [HttpPut("{cartId:guid}")]
        public async Task<ActionResult> SetProductQuantity([FromRoute]Guid cartId, [FromBody]CartSetProductQuantityDto dto)
        {
            await _cartService.SetProductQuantity(cartId, dto.ProductId, dto.Quantity);
            return NoContent();
        }
        [HttpDelete("{cartId:guid}/clear")]
        public async Task<ActionResult> ClearCart([FromRoute] Guid cartId)
        {
            await _cartService.ClearCartAsync(cartId);
            return NoContent();
        }
        [HttpPost("{cartId:guid}/checkout")]
        public async Task<ActionResult> Checkout([FromRoute]Guid cartId)
        {
            await _cartService.CheckoutAsync(cartId);
            return NoContent();
        }
    }
}
