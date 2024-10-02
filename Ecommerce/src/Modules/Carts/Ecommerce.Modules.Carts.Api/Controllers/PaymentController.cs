using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Api.Controllers
{
    internal class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PaymentDto>>>> BrowsePayments()
        {
            var payments = await _paymentService.BrowseAsync();
            return Ok(new ApiResponse<IEnumerable<PaymentDto>>(HttpStatusCode.OK, payments));
        }
        [HttpGet("available")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PaymentDto>>>> BrowseAvailablePayments()
        {
            var payments = await _paymentService.BrowseAvailableAsync();
            return Ok(new ApiResponse<IEnumerable<AvailablePaymentDto>>(HttpStatusCode.OK, payments));
        }
        [HttpPut("{paymentId:guid}/activate")]
        public async Task<ActionResult> ActivatePayment([FromRoute]Guid paymentId)
        {
            await _paymentService.SetActivePaymentMethod(true, paymentId);
            return NoContent();
        }
        [HttpPut("{paymentId:guid}/deactivate")]
        public async Task<ActionResult> DeactivatePayment([FromRoute] Guid paymentId)
        {
            await _paymentService.SetActivePaymentMethod(false, paymentId);
            return NoContent();
        }
    }
}
