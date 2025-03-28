using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.BrowseInvoices;
using Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.CreateInvoice;
using Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.DeleteInvoice;
using Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.DownloadInvoice;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/" + OrdersModule.BasePath + "/orders/{orderId:guid}/[controller]")]
    internal class InvoicesController : BaseController
    {
        public InvoicesController(IMediator mediator) : base(mediator)
        {
        }

        [SwaggerOperation("Gets cursor paginated invoices")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns cursor paginated result for invoices.", typeof(ApiResponse<CursorPagedResult<InvoiceBrowseDto, InvoiceCursorDto>>))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [Route("/api/v{v:apiVersion}/" + OrdersModule.BasePath + "/[controller]")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<InvoiceBrowseDto, InvoiceCursorDto>>>> BrowseInvoices([FromQuery] BrowseInvoices query, 
            CancellationToken cancellationToken)
            => CursorPagedResult(await _mediator.Send(query, cancellationToken));

        [SwaggerOperation("Creates an invoice")]
        [SwaggerResponse(StatusCodes.Status200OK, "Creates a manufacturer and returns it's invoice number.", typeof(string))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPost]
        public async Task<ActionResult<string>> CreateInvoice([FromRoute] Guid orderId, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateInvoice(orderId), cancellationToken);
            return result;
        }

        [SwaggerOperation("Downloads an invoice")]
        [SwaggerResponse(StatusCodes.Status200OK, "Downloads an invoice for a specified order.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpGet]
        public async Task<FileStreamResult> DownloadInvoice([FromRoute]Guid orderId, 
            CancellationToken cancellationToken)
            => await _mediator.Send(new DownloadInvoice(orderId), cancellationToken);

        [SwaggerOperation("Deletes an invoice")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpDelete]
        public async Task<ActionResult> DeleteInvoice([FromRoute]Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteInvoice(orderId), cancellationToken);
            return NoContent();
        }
    }
}
