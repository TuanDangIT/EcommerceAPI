using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.CancelOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.CreateInvoice;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.CreateLabel;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.DownloadLabel;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.GetOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.ReturnOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.SetParcels;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.SubmitComplaint;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    [ApiVersion(1)]
    internal class OrderController : BaseController
    {
        public OrderController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<OrderBrowseDto, OrderCursorDto>>>> BrowseOrders([FromQuery] BrowseOrders query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<CursorPagedResult<OrderBrowseDto, OrderCursorDto>>(HttpStatusCode.OK, result));
        }
        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<ApiResponse<OrderDetailsDto>>> GetOrder([FromRoute]Guid orderId)
            => OkOrNotFound<OrderDetailsDto, Order>(await _mediator.Send(new GetOrder(orderId)));
        [HttpPost("{orderId:guid}/cancel")]
        public async Task<ActionResult> CancelOrder([FromRoute] Guid orderId)
        {
            await _mediator.Send(new CancelOrder(orderId));
            return NoContent();
        }
        [HttpPost("{orderId:guid}/return")]
        public async Task<ActionResult> ReturnOrder([FromRoute] Guid orderId, [FromBody]ReturnOrder command)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpPost("{orderId:guid}/submit-complaint")]
        public async Task<ActionResult> SubmitComplaint([FromRoute] Guid orderId, [FromForm]SubmitComplaint command)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpGet("{orderId:guid}/label")]
        public async Task<ActionResult<IFormFile>> DownloadLabel([FromRoute]Guid orderId)
        {
            var result = await _mediator.Send(new DownloadLabel(orderId));
            return File(result.FileStream, result.MimeType, result.FileName);
        }
        [HttpPost("{orderId:guid}/label")]
        public async Task<ActionResult> CreateLabel([FromRoute]Guid orderId)
        {
            await _mediator.Send(new CreateLabel(orderId));
            return NoContent();
        }
        [HttpPut("{orderId:guid}/parcel")]
        public async Task<ActionResult> SetParcels([FromRoute]Guid orderId, [FromBody]SetParcels command)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpPost("{orderId:guid}/invoice")]
        public async Task<ActionResult> CreateInvoice([FromRoute]Guid orderId)
        {
            var result = await _mediator.Send(new CreateInvoice(orderId));
            return Ok(result);
        }
    }
}
