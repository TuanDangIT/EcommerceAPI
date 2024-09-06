﻿using Ecommerce.Modules.Carts.Core.DTO;
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
    internal class CheckoutCartController : BaseController
    {
        private readonly ICheckoutCartService _checkoutCartService;
        private const string WebhookSecret = "whsec_bcd675ccca84c19fe21093304007e2eff80eebfea4b99552b4097f438ca22955";

        public CheckoutCartController(ICheckoutCartService checkoutCartService)
        {
            _checkoutCartService = checkoutCartService;
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<CheckoutCartDto>>> GetCheckoutCart([FromRoute]Guid id)
        {
            var checkoutCart = await _checkoutCartService.GetAsync(id);
            return Ok(new ApiResponse<CheckoutCartDto>(HttpStatusCode.OK, "success", checkoutCart));
        }
        [HttpPut("{checkoutCartId:guid}/payment")]
        public async Task<ActionResult> SetPayment([FromRoute]Guid checkoutCartId, [FromBody]Guid paymentId)
        {
            await _checkoutCartService.SetPaymentAsync(checkoutCartId, paymentId);
            return NoContent();
        }
        [HttpPut("{checkoutCartId:guid}/shipment")]
        public async Task<ActionResult> SetShipment([FromRoute] Guid checkoutCartId, [FromBody]ShipmentDto paymentId)
        {
            await _checkoutCartService.SetShipmentAsync(checkoutCartId, paymentId);
            return NoContent();
        }
        [HttpPost("{id:guid}")]
        public async Task<ActionResult<ApiResponse<CheckoutStripeSessionDto>>> PlaceOrder([FromRoute]Guid id)
        {
            var checkoutUrl = await _checkoutCartService.PlaceOrderAsync(id);
            return Ok(new ApiResponse<CheckoutStripeSessionDto>(HttpStatusCode.OK, "success", checkoutUrl));
        }
        [HttpPost("webhook")]
        public async Task<ActionResult> WebhookHandler()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WebhookSecret);
                if(stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    Console.WriteLine(session?.Id);
                    await _checkoutCartService.HandleCheckoutSessionCompleted(session);
                }
                return Ok();
            }
            catch(StripeException)
            {
                return BadRequest();
            }
        }
    }
}
