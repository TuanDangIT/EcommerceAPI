using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Invoices.DTO;
using Ecommerce.Modules.Orders.Application.Invoices.Features.BrowseInvoices;
using Ecommerce.Modules.Orders.Application.Invoices.Features.DeleteInvoice;
using Ecommerce.Modules.Orders.Application.Invoices.Features.DownloadInvoice;
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
        //[HttpPost("create-invoice")]
        //public async Task<ActionResult<IFormFile>> CreateInvoice()
        //{
        //    var result = await _mediator.Send(new CreateInvoice());
        //    return File(result, "application/pdf", "sample.pdf");
        //}
        [HttpGet("{invoiceId:int}")]
        public async Task<ActionResult<ApiResponse<InvoiceDetailsDto>>> DownloadInvoice([FromRoute]int invoiceId)
        {
            var result = await _mediator.Send(new DownloadInvoice(invoiceId));
            return File(result.FileStream, result.MimeType, result.FileName);
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<InvoiceBrowseDto, InvoiceCursorDto>>>> BrowseInvoices([FromQuery] BrowseInvoices query)
            => CursorPagedResult(await _mediator.Send(query));
        [HttpDelete("{invoiceId:int}")]
        public async Task<ActionResult> DeleteInvoice([FromRoute]int invoiceId)
        {
            await _mediator.Send(new DeleteInvoice(invoiceId));
            return NoContent();
        }
    }
}
