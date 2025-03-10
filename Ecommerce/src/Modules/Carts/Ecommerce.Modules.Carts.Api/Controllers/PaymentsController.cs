using Asp.Versioning;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
    internal class PaymentsController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [SwaggerOperation("Gets offset paginated payments")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for payments.", typeof(ApiResponse<IEnumerable<PaymentDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PaymentDto>>>> BrowsePayments(CancellationToken cancellationToken)
        {
            var payments = await _paymentService.BrowseAsync(cancellationToken);
            return Ok(new ApiResponse<IEnumerable<PaymentDto>>(HttpStatusCode.OK, payments));
        }

        [SwaggerOperation("Gets offset paginated payments")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for active/available payments.", typeof(ApiResponse<IEnumerable<PaymentDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [AllowAnonymous]
        [HttpGet("available")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PaymentDto>>>> BrowseAvailablePayments(CancellationToken cancellationToken)
        {
            var payments = await _paymentService.BrowseAvailableAsync(cancellationToken);
            return Ok(new ApiResponse<IEnumerable<PaymentDto>>(HttpStatusCode.OK, payments));
        }

        [SwaggerOperation("Activates payment", "Makes the payment not available for customers")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{paymentId:guid}/activate")]
        public async Task<ActionResult> ActivatePayment([FromRoute] Guid paymentId, CancellationToken cancellationToken)
        {
            await _paymentService.SetActiveAsync(paymentId, true, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Deactivates payment")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{paymentId:guid}/deactivate")]
        public async Task<ActionResult> DeactivatePayment([FromRoute] Guid paymentId, CancellationToken cancellationToken)
        {
            await _paymentService.SetActiveAsync(paymentId, false, cancellationToken);
            return NoContent();
        }
    }
}
