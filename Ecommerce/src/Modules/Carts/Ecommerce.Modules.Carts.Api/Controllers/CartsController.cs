using Asp.Versioning;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [SwaggerOperation("Creates a cart")]
        [SwaggerResponse(StatusCodes.Status201Created, "Creates a cart and returns it's id.", typeof(Guid))]
        [HttpPost]
        public async Task<ActionResult> CreateCart()
        {
            var cartId = await _cartService.CreateAsync();
            return CreatedAtAction(nameof(GetCart), new { cartId }, cartId);
        }

        [SwaggerOperation("Gets specific cart")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a specified cart by id.", typeof(ApiResponse<CartDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Cart was not found.")]
        [HttpGet("{cartId:guid}")]
        public async Task<ActionResult<ApiResponse<CartDto>>> GetCart([FromRoute]Guid cartId, CancellationToken cancellationToken)
            => OkOrNotFound(await _cartService.GetAsync(cartId, cancellationToken), "Cart", cartId.ToString());

        [SwaggerOperation("Adds a product to a cart")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpPost("{cartId:guid}/products")]
        public async Task<ActionResult> AddProduct([FromRoute]Guid cartId, [FromBody]CartAddProductDto dto, CancellationToken cancellationToken)
        {
            await _cartService.AddProductAsync(cartId, dto.ProductId, dto.Quantity, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Removes a product from cart")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpDelete("{cartId:guid}/products")]
        public async Task<ActionResult> RemoveProduct([FromRoute]Guid cartId, [FromBody]CartRemoveProductDto dto, CancellationToken cancellationToken)
        {
            await _cartService.RemoveProductAsync(cartId, dto.ProductId, dto.Quantity, cancellationToken);
            return NoContent();
        }
        [SwaggerOperation("Sets product' quantity in a cart")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpPut("{cartId:guid}/products")]
        public async Task<ActionResult> SetProductQuantity([FromRoute]Guid cartId, [FromBody]CartSetProductQuantityDto dto, CancellationToken cancellationToken)
        {
            await _cartService.SetProductQuantityAsync(cartId, dto.ProductId, dto.Quantity, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Clears a cart of produts")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpDelete("{cartId:guid}/products/clear")]
        public async Task<ActionResult> ClearCart([FromRoute] Guid cartId, CancellationToken cancellationToken)
        {
            await _cartService.ClearAsync(cartId, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Adds a discount to a cart")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpPut("{cartId:guid}/discounts/{code}")]
        public async Task<ActionResult> AddDiscount([FromRoute] Guid cartId, [FromRoute] string code, CancellationToken cancellationToken)
        {
            await _cartService.AddDiscountAsync(cartId, code, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Removes a discount from a cart")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpDelete("{cartId:guid}/discounts")]
        public async Task<ActionResult> RemoveDiscount([FromRoute] Guid cartId, CancellationToken cancellationToken)
        {
            await _cartService.RemoveDiscountAsync(cartId, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Checkout")]
        [SwaggerResponse(StatusCodes.Status201Created, "Creates a checkout cart.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpPost("{cartId:guid}/checkout")]
        public async Task<ActionResult> Checkout([FromRoute]Guid cartId, CancellationToken cancellationToken)
        {
            var checkoutCartId = await _cartService.CheckoutAsync(cartId, cancellationToken);
            return CreatedAtAction(nameof(CheckoutCartsController.GetCheckoutCart), "CheckoutCarts", new { checkoutCartId }, checkoutCartId);
        }
    }
}
