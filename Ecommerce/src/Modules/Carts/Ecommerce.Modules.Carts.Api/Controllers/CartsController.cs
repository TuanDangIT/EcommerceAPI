using Asp.Versioning;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Api.Controllers
{
    [ApiVersion(1)]
    internal class CartsController : BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost]
        public async Task<ActionResult> CreateCart()
        {
            var cartId = await _cartService.CreateAsync();
            return CreatedAtAction(nameof(GetCart), new { cartId }, cartId);
        }
        [HttpGet("{cartId:guid}")]
        public async Task<ActionResult<ApiResponse<CartDto?>>> GetCart([FromRoute]Guid cartId, CancellationToken cancellationToken)
            => OkOrNotFound<CartDto?>(await _cartService.GetAsync(cartId, cancellationToken), "Cart");
        [HttpPost("{cartId:guid}")]
        public async Task<ActionResult> AddProduct([FromRoute]Guid cartId, [FromBody]CartAddProductDto dto, CancellationToken cancellationToken)
        {
            await _cartService.AddProductAsync(cartId, dto.ProductId, dto.Quantity, cancellationToken);
            return NoContent();
        }
        [HttpDelete("{cartId:guid}")]
        public async Task<ActionResult> RemoveProduct([FromRoute]Guid cartId, [FromBody]CartRemoveProductDto dto, CancellationToken cancellationToken)
        {
            await _cartService.RemoveProductAsync(cartId, dto.ProductId, dto.Quantity, cancellationToken);
            return NoContent();
        }
        [HttpPut("{cartId:guid}")]
        public async Task<ActionResult> SetProductQuantity([FromRoute]Guid cartId, [FromBody]CartSetProductQuantityDto dto, CancellationToken cancellationToken)
        {
            await _cartService.SetProductQuantityAsync(cartId, dto.ProductId, dto.Quantity, cancellationToken);
            return NoContent();
        }
        [HttpDelete("{cartId:guid}/clear")]
        public async Task<ActionResult> ClearCart([FromRoute] Guid cartId, CancellationToken cancellationToken)
        {
            await _cartService.ClearAsync(cartId, cancellationToken);
            return NoContent();
        }
        [HttpPut("{cartId:guid}/discount")]
        public async Task<ActionResult> AddDiscount([FromRoute] Guid cartId, [FromBody] string code, CancellationToken cancellationToken)
        {
            await _cartService.AddDiscountAsync(cartId, code, cancellationToken);
            return NoContent();
        }
        [HttpDelete("{cartId:guid}/discount")]
        public async Task<ActionResult> RemoveDiscount([FromRoute] Guid cartId, CancellationToken cancellationToken)
        {
            await _cartService.RemoveDiscountAsync(cartId, cancellationToken);
            return NoContent();
        }
        [HttpPost("{cartId:guid}/checkout")]
        public async Task<ActionResult> Checkout([FromRoute]Guid cartId, CancellationToken cancellationToken)
        {
            var checkoutCartId = await _cartService.CheckoutAsync(cartId, cancellationToken);
            return CreatedAtAction(nameof(CheckoutCartsController.GetCheckoutCart), typeof(CheckoutCart).Name, new { checkoutCartId }, checkoutCartId);
        }
    }
}
