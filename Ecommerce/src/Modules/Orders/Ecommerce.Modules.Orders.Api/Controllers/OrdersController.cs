using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.CreateInvoice;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.CancelOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.GetOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.HandleOrderDelivered;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.HandleOrderShipped;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.ReturnOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.SubmitComplaint;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.WriteAdditionalInformation;
using Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.CreateShipment;
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
    internal class OrdersController : BaseController
    {
        public OrdersController(IMediator mediator) : base(mediator)
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
        [HttpPut("{orderId:guid}/additional-information")]
        public async Task<ActionResult> WriteAdditionalInformation([FromRoute]Guid orderId, [FromBody]string additionalInformation)
        {
            await _mediator.Send(new WriteAdditionalInformation(orderId, additionalInformation));
            return NoContent();
        }
        [HttpPost("/api/webhooks/v{v:apiVersion}/" + OrdersModule.BasePath + "/[controller]/order-delivered")]
        public async Task<ActionResult> InPostParcelDeliveredWebhookHandler()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            await _mediator.Send(new HandleOrderDelivered(json));
            return Ok();
        }
        [HttpPost("/api/webhooks/v{v:apiVersion}/" + OrdersModule.BasePath + "/[controller]/order-shipped")]
        public async Task<ActionResult> InPostParcelShippedWebhookHandler()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            await _mediator.Send(new HandlerOrderShipped(json));
            return Ok();
        }
    }
}
