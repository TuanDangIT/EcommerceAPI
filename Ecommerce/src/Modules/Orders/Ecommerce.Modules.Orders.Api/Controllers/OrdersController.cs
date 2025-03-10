using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Complaints.DTO;
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
using Swashbuckle.AspNetCore.Annotations;
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

        [SwaggerOperation("Gets cursor paginated orders")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns cursor paginated result for orders.", typeof(ApiResponse<CursorPagedResult<OrderBrowseDto, OrderCursorDto>>))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<OrderBrowseDto, OrderCursorDto>>>> BrowseOrders([FromQuery] BrowseOrders query, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<CursorPagedResult<OrderBrowseDto, OrderCursorDto>>(HttpStatusCode.OK, result));
        }

        [SwaggerOperation("Get a specific order")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a specific order by id.", typeof(ApiResponse<OrderDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order was not found")]
        [AllowAnonymous]
        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<ApiResponse<OrderDetailsDto>>> GetOrder([FromRoute]Guid orderId, CancellationToken cancellationToken = default)
            => OkOrNotFound<OrderDetailsDto, Order>(await _mediator.Send(new GetOrder(orderId), cancellationToken));

        [SwaggerOperation("Creates an order")]
        [SwaggerResponse(StatusCodes.Status201Created, "Creates an order and returns it's identifier", typeof(Guid))]
        [HttpPost]
        public async Task<ActionResult> CreateDraftOrder()
        {
            var orderId = await _mediator.Send(new CreateDraftOrder());
            return CreatedAtAction(nameof(GetOrder), new { orderId }, orderId);
        }

        [SwaggerOperation("Submits order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{orderId:guid}/submit-order")]
        public async Task<ActionResult> SubmitOrder([FromRoute]Guid orderId)
        {
            await _mediator.Send(new SubmitOrder(orderId));
            return NoContent();
        }

        [SwaggerOperation("Adds product to specified order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPost("{orderId:guid}/products")]
        public async Task<ActionResult> AddProduct([FromRoute]Guid orderId, [FromBody]AddProduct command)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command);
            return NoContent();
        }

        [SwaggerOperation("Deletes an order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete("{orderId:guid}/products")]
        public async Task<ActionResult> RemoveProduct([FromRoute] Guid orderId, [FromBody] RemoveProduct command)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command);
            return NoContent();
        }

        [SwaggerOperation("Updates a products's price for specified order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{orderId:guid}/products/{productId:int}/unit-price")]
        public async Task<ActionResult> EditProductUnitPrice([FromRoute]Guid orderId, [FromRoute]int productId, [FromBody]decimal unitPrice)
        {
            await _mediator.Send(new EditProductUnitPrice(orderId, productId, unitPrice));
            return NoContent();
        }

        [SwaggerOperation("Cancels a order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [AllowAnonymous]
        [HttpPut("{orderId:guid}/cancel")]
        public async Task<ActionResult> CancelOrder([FromRoute] Guid orderId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new CancelOrder(orderId), cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Returns a order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [AllowAnonymous]
        [HttpPost("{orderId:guid}/return")]
        public async Task<ActionResult> ReturnOrder([FromRoute] Guid orderId, [FromBody]ReturnOrder command, CancellationToken cancellationToken = default)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Submits a complaint")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [AllowAnonymous]
        [HttpPost("{orderId:guid}/submit-complaint")]
        public async Task<ActionResult> SubmitComplaint([FromRoute] Guid orderId, [FromForm]SubmitComplaint command, CancellationToken cancellationToken = default)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Updates a order's additional information")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{orderId:guid}/additional-information")]
        public async Task<ActionResult> WriteAdditionalInformation([FromRoute]Guid orderId, [FromBody]string additionalInformation, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new WriteAdditionalInformation(orderId, additionalInformation), cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Webhook for updating order if delivered")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("/api/webhooks/v{v:apiVersion}/" + OrdersModule.BasePath + "/[controller]/order-delivered")]
        public async Task<ActionResult> InPostParcelDeliveredWebhookHandler()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            await _mediator.Send(new HandleOrderDelivered(json));
            return Ok();
        }

        [SwaggerOperation("Webhook for updating order if shipped")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
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
