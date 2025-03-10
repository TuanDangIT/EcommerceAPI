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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/" + OrdersModule.BasePath + "/orders/{orderId:guid}/[controller]")]
    internal class InvoiceController : BaseController
    {
        public InvoiceController(IMediator mediator) : base(mediator)
        {
        }

        [SwaggerOperation("Gets cursor paginated invoices")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns cursor paginated result for invoices.", typeof(ApiResponse<CursorPagedResult<InvoiceBrowseDto, InvoiceCursorDto>>))]
        [Route("/api/v{v:apiVersion}/" + OrdersModule.BasePath + "/[controller]")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<InvoiceBrowseDto, InvoiceCursorDto>>>> BrowseInvoices([FromQuery] BrowseInvoices query, 
            CancellationToken cancellationToken = default)
            => CursorPagedResult(await _mediator.Send(query, cancellationToken));

        [SwaggerOperation("Creates an invoice")]
        [SwaggerResponse(StatusCodes.Status200OK, "Creates a manufacturer and returns it's invoice number.", typeof(string))]
        [HttpPost]
        public async Task<ActionResult<string>> CreateInvoice([FromRoute] Guid orderId, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new CreateInvoice(orderId), cancellationToken);
            return Ok(result);
        }

        [SwaggerOperation("Downloads an invoice")]
        [SwaggerResponse(StatusCodes.Status200OK, "Downloads an invoice for a specified order.", typeof(ApiResponse<InvoiceDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<InvoiceDetailsDto>>> DownloadInvoice([FromRoute]Guid orderId, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DownloadInvoice(orderId), cancellationToken);
            return File(result.FileStream, result.MimeType, result.FileName);
        }

        [SwaggerOperation("Deletes an invoice")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete]
        public async Task<ActionResult> DeleteInvoice([FromRoute]Guid orderId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteInvoice(orderId), cancellationToken);
            return NoContent();
        }
    }
}
