using Asp.Versioning;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Api.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PaymentDto>>>> BrowsePayments(CancellationToken cancellationToken)
        {
            var payments = await _paymentService.BrowseAsync(cancellationToken);
            return Ok(new ApiResponse<IEnumerable<PaymentDto>>(HttpStatusCode.OK, payments));
        }
        [AllowAnonymous]
        [HttpGet("available")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PaymentDto>>>> BrowseAvailablePayments(CancellationToken cancellationToken)
        {
            var payments = await _paymentService.BrowseAvailableAsync(cancellationToken);
            return Ok(new ApiResponse<IEnumerable<PaymentDto>>(HttpStatusCode.OK, payments));
        }
        [HttpPut("{paymentId:guid}/activate")]
        public async Task<ActionResult> ActivatePayment([FromRoute] Guid paymentId, CancellationToken cancellationToken)
        {
            await _paymentService.SetActiveAsync(paymentId, true, cancellationToken);
            return NoContent();
        }
        [HttpPut("{paymentId:guid}/deactivate")]
        public async Task<ActionResult> DeactivatePayment([FromRoute] Guid paymentId, CancellationToken cancellationToken)
        {
            await _paymentService.SetActiveAsync(paymentId, false, cancellationToken);
            return NoContent();
        }
    }
}
