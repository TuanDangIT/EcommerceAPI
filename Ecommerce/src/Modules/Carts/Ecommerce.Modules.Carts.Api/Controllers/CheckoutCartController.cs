using Asp.Versioning;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Api.Controllers
{
    [ApiVersion(1)]
    internal class CheckoutCartController : BaseController
    {
        private readonly ICheckoutCartService _checkoutCartService;

        public CheckoutCartController(ICheckoutCartService checkoutCartService)
        {
            _checkoutCartService = checkoutCartService;
        }
        [HttpGet("{checkoutCartId:guid}")]
        public async Task<ActionResult<ApiResponse<CheckoutCartDto?>>> GetCheckoutCart([FromRoute]Guid checkoutCartId, CancellationToken cancellationToken)
            => OkOrNotFound<CheckoutCartDto?>(await _checkoutCartService.GetAsync(checkoutCartId, cancellationToken), "Checkout cart");
        [HttpPut("{checkoutCartId:guid}/customer")]
        public async Task<ActionResult> SetCustomer([FromRoute] Guid checkoutCartId, [FromBody]CustomerDto customerDto, CancellationToken cancellationToken)
        {
            await _checkoutCartService.SetCustomerAsync(checkoutCartId, customerDto, cancellationToken);
            return NoContent();
        }
        [HttpPut("{checkoutCartId:guid}/payment")]
        public async Task<ActionResult> SetPayment([FromRoute]Guid checkoutCartId, [FromBody]Guid paymentId, CancellationToken cancellationToken)
        {
            await _checkoutCartService.SetPaymentAsync(checkoutCartId, paymentId, cancellationToken);
            return NoContent();
        }
        [HttpPut("{checkoutCartId:guid}/shipment")]
        public async Task<ActionResult> SetShipment([FromRoute] Guid checkoutCartId, [FromBody]ShipmentDto shipmentDto, CancellationToken cancellationToken)
        {
            await _checkoutCartService.SetShipmentAsync(checkoutCartId, shipmentDto, cancellationToken);
            return NoContent();
        }
        [HttpPut("{checkoutCartId:guid}/additional-information")]
        public async Task<ActionResult> SetAdditionalInformation([FromRoute] Guid checkoutCartId, [FromBody] string additionalInformation, CancellationToken cancellationToken)
        {
            await _checkoutCartService.SetAdditionalInformationAsync(checkoutCartId, additionalInformation, cancellationToken);
            return NoContent();
        }
        [HttpPut("{checkoutCartId:guid}/checkoutcart-details")]
        public async Task<ActionResult> SetCheckoutCartDetails([FromRoute] Guid checkoutCartId, [FromBody]CheckoutCartSetDetailsDto checkoutCartSetDetailsDto, 
            CancellationToken cancellationToken)
        {
            await _checkoutCartService.SetCheckoutCartDetailsAsync(checkoutCartId, checkoutCartSetDetailsDto, cancellationToken);
            return NoContent();
        }
        [HttpPost("{checkoutCartId:guid}/place-order")]
        public async Task<ActionResult<ApiResponse<CheckoutStripeSessionDto>>> PlaceOrder([FromRoute]Guid checkoutCartId, CancellationToken cancellationToken)
        {
            var checkoutUrl = await _checkoutCartService.PlaceOrderAsync(checkoutCartId, cancellationToken);
            return Ok(new ApiResponse<CheckoutStripeSessionDto>(HttpStatusCode.OK, checkoutUrl));
        }
        //[HttpPut("{checkoutCartId:guid}/discount")]
        //public async Task<ActionResult> AddDiscount([FromRoute] Guid checkoutCartId, [FromBody] string code, CancellationToken cancellationToken)
        //{
        //    await _checkoutCartService.AddDiscountAsync(checkoutCartId, code, cancellationToken);
        //    return NoContent();
        //}
        //[HttpDelete("{checkoutCartId:guid}/discount")]
        //public async Task<ActionResult> RemoveDiscount([FromRoute] Guid checkoutCartId, CancellationToken cancellationToken)
        //{
        //    await _checkoutCartService.RemoveDiscountAsync(checkoutCartId, cancellationToken);
        //    return NoContent();
        //}
        [HttpPost("/api/webhooks/v{v:apiVersion}/" + CartsModule.BasePath + "/[controller]/checkout-session-completed")]
        public async Task<ActionResult> HandleCheckoutSessionCompleted()
        {
            await _checkoutCartService.HandleCheckoutSessionCompletedAsync(await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(), 
                Request.Headers["Stripe-Signature"]);
            return Ok();
        }
    }
}
