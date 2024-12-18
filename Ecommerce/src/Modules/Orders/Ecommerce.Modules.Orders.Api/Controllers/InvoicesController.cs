using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.BrowseInvoices;
using Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.CreateInvoice;
using Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.DeleteInvoice;
using Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.DownloadInvoice;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    [ApiVersion(1)]
    internal class InvoiceController : BaseController
    {
        public InvoiceController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<InvoiceBrowseDto, InvoiceCursorDto>>>> BrowseInvoices([FromQuery] BrowseInvoices query, 
            CancellationToken cancellationToken = default)
            => CursorPagedResult(await _mediator.Send(query, cancellationToken));
        [HttpPost("{orderId:guid}")]
        public async Task<ActionResult> CreateInvoice([FromRoute] Guid orderId, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new CreateInvoice(orderId), cancellationToken);
            return Ok(result);
        }
        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<ApiResponse<InvoiceDetailsDto>>> DownloadInvoice([FromRoute]Guid orderId, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DownloadInvoice(orderId), cancellationToken);
            return File(result.FileStream, result.MimeType, result.FileName);
        }
        [HttpDelete("{orderId:guid}")]
        public async Task<ActionResult> DeleteInvoice([FromRoute]Guid orderId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteInvoice(orderId), cancellationToken);
            return NoContent();
        }
    }
}
