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
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.AddProduct;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.CreateDraftOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.EditProductUnitPrice;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.RemoveProduct;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.SubmitOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.WriteAdditionalInformation;
using Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.CreateShipment;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class OrdersController : BaseController
    {
        public OrdersController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<OrderBrowseDto, OrderCursorDto>>>> BrowseOrders([FromQuery] BrowseOrders query, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<CursorPagedResult<OrderBrowseDto, OrderCursorDto>>(HttpStatusCode.OK, result));
        }
        [AllowAnonymous]
        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<ApiResponse<OrderDetailsDto>>> GetOrder([FromRoute]Guid orderId, CancellationToken cancellationToken = default)
            => OkOrNotFound<OrderDetailsDto, Order>(await _mediator.Send(new GetOrder(orderId), cancellationToken));
        [HttpPost()]
        public async Task<ActionResult> CreateDraftOrder()
        {
            var orderId = await _mediator.Send(new CreateDraftOrder());
            return CreatedAtAction(nameof(GetOrder), new { orderId }, orderId);
        }
        [HttpPut("{orderId:guid}/submit")]
        public async Task<ActionResult> SubmitOrder([FromRoute]Guid orderId)
        {
            await _mediator.Send(new SubmitOrder(orderId));
            return NoContent();
        }
        [HttpPost("{orderId:guid}/products")]
        public async Task<ActionResult> AddProduct([FromRoute]Guid orderId, [FromBody]AddProduct command)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{orderId:guid}/products")]
        public async Task<ActionResult> RemoveProduct([FromRoute] Guid orderId, [FromBody] RemoveProduct command)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpPut("{orderId:guid}/products/{productId:int}/unit-price")]
        public async Task<ActionResult> EditProductUnitPrice([FromRoute]Guid orderId, [FromRoute]int productId, [FromBody]decimal unitPrice)
        {
            await _mediator.Send(new EditProductUnitPrice(orderId, productId, unitPrice));
            return NoContent();
        }
        [AllowAnonymous]
        [HttpPost("{orderId:guid}/cancel")]
        public async Task<ActionResult> CancelOrder([FromRoute] Guid orderId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new CancelOrder(orderId), cancellationToken);
            return NoContent();
        }
        [AllowAnonymous]
        [HttpPost("{orderId:guid}/return")]
        public async Task<ActionResult> ReturnOrder([FromRoute] Guid orderId, [FromBody]ReturnOrder command, CancellationToken cancellationToken = default)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        [AllowAnonymous]
        [HttpPost("{orderId:guid}/submit-complaint")]
        public async Task<ActionResult> SubmitComplaint([FromRoute] Guid orderId, [FromForm]SubmitComplaint command, CancellationToken cancellationToken = default)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        [HttpPut("{orderId:guid}/additional-information")]
        public async Task<ActionResult> WriteAdditionalInformation([FromRoute]Guid orderId, [FromBody]string additionalInformation, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new WriteAdditionalInformation(orderId, additionalInformation), cancellationToken);
            return NoContent();
        }
        [AllowAnonymous]
        [HttpPost("/api/webhooks/v{v:apiVersion}/" + OrdersModule.BasePath + "/[controller]/order-delivered")]
        public async Task<ActionResult> InPostParcelDeliveredWebhookHandler()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            await _mediator.Send(new HandleOrderDelivered(json));
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("/api/webhooks/v{v:apiVersion}/" + OrdersModule.BasePath + "/[controller]/order-shipped")]
        public async Task<ActionResult> InPostParcelShippedWebhookHandler()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            await _mediator.Send(new HandlerOrderShipped(json));
            return Ok();
        }
    }
}
