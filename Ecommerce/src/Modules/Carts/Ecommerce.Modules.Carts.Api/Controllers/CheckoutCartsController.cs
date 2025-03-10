using Asp.Versioning;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Api.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/" + CartsModule.BasePath + "/checkout-cart")]
    internal class CheckoutCartsController : BaseController
    {
        private readonly ICheckoutCartService _checkoutCartService;

        public CheckoutCartsController(ICheckoutCartService checkoutCartService)
        {
            _checkoutCartService = checkoutCartService;
        }

        [SwaggerOperation("Gets a specified checkout cart")]
        [SwaggerResponse(StatusCodes.Status200OK, "Retrieves a specified checkout cart by id.", typeof(ApiResponse<CheckoutCartDto?>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Checkout cart was not found.", Type = typeof(ProblemDetails))]
        [HttpGet("{checkoutCartId:guid}")]
        public async Task<ActionResult<ApiResponse<CheckoutCartDto?>>> GetCheckoutCart([FromRoute]Guid checkoutCartId, CancellationToken cancellationToken)
            => OkOrNotFound<CheckoutCartDto?>(await _checkoutCartService.GetAsync(checkoutCartId, cancellationToken), "Checkout cart", checkoutCartId.ToString());

        [SwaggerOperation("Sets customer details for a cart")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{checkoutCartId:guid}/customer")]
        public async Task<ActionResult> SetCustomer([FromRoute] Guid checkoutCartId, [FromBody]CustomerDto customerDto, CancellationToken cancellationToken)
        {
            await _checkoutCartService.SetCustomerAsync(checkoutCartId, customerDto, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Sets payment for a cart")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{checkoutCartId:guid}/payment")]
        public async Task<ActionResult> SetPayment([FromRoute]Guid checkoutCartId, [FromBody]Guid paymentId, CancellationToken cancellationToken)
        {
            await _checkoutCartService.SetPaymentAsync(checkoutCartId, paymentId, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Sets shipment details for a cart")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{checkoutCartId:guid}/shipment")]
        public async Task<ActionResult> SetShipment([FromRoute] Guid checkoutCartId, [FromBody]ShipmentFillDto shipmentFillDto, CancellationToken cancellationToken)
        {
            await _checkoutCartService.FillShipmentDetailsAsync(checkoutCartId, shipmentFillDto, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Sets additional information for a cart")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{checkoutCartId:guid}/additional-information")]
        public async Task<ActionResult> SetAdditionalInformation([FromRoute] Guid checkoutCartId, [FromBody] string additionalInformation, CancellationToken cancellationToken)
        {
            await _checkoutCartService.SetAdditionalInformationAsync(checkoutCartId, additionalInformation, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Sets customer id for a cart if user is logged in")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{checkoutCartId:guid}/customer-id")]
        public async Task<ActionResult> SetCustomerId([FromRoute] Guid checkoutCartId, [FromBody] Guid? customerId, CancellationToken cancellationToken)
        {
            await _checkoutCartService.SetCustomerIdAsync(checkoutCartId, customerId, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Sets checkout cart details for a cart", "Sets checkout cart details for a cart which include payment, shipment and customer.")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{checkoutCartId:guid}")]
        public async Task<ActionResult> SetCheckoutCartDetails([FromRoute] Guid checkoutCartId, [FromBody]CheckoutCartSetDetailsDto checkoutCartSetDetailsDto, 
            CancellationToken cancellationToken)
        {
            await _checkoutCartService.SetCheckoutCartDetailsAsync(checkoutCartId, checkoutCartSetDetailsDto, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Places order")]
        [SwaggerResponse(StatusCodes.Status200OK, "Places order and returns stripe session details.", typeof(ApiResponse<CheckoutStripeSessionDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPost("{checkoutCartId:guid}/place-order")]
        public async Task<ActionResult<ApiResponse<CheckoutStripeSessionDto>>> PlaceOrder([FromRoute]Guid checkoutCartId, CancellationToken cancellationToken)
        {
            var checkoutUrl = await _checkoutCartService.PlaceOrderAsync(checkoutCartId, cancellationToken);
            return Ok(new ApiResponse<CheckoutStripeSessionDto>(HttpStatusCode.OK, checkoutUrl));
        }

        [SwaggerOperation("Webhook for updating payment process")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpPost("/api/webhooks/v{v:apiVersion}/" + CartsModule.BasePath + "/[controller]/checkout-session-completed")]
        public async Task<ActionResult> HandleCheckoutSessionCompleted()
        {
            await _checkoutCartService.HandleCheckoutSessionCompletedAsync(await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(), 
                Request.Headers["Stripe-Signature"]);
            return Ok();
        }
    }
}
