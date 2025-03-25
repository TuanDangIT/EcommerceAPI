using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaints.BrowseComplaintsByCustomerId;
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
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.BrowseOrdersByCustomerId;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.CreateDraftOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.DeleteOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.EditProductUnitPrice;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.RemoveProduct;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.SubmitOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.WriteAdditionalInformation;
using Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.CreateShipment;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
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
    [ApiVersion(1)]
    internal class OrdersController : BaseController
    {
        public OrdersController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Gets cursor paginated orders")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns cursor paginated result for orders.", typeof(ApiResponse<CursorPagedResult<OrderBrowseDto, OrderCursorDto>>))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<OrderBrowseDto, OrderCursorDto>>>> BrowseOrders([FromQuery] BrowseOrders query, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<CursorPagedResult<OrderBrowseDto, OrderCursorDto>>(HttpStatusCode.OK, result));
        }

        [Authorize(Roles = "Customer")]
        [SwaggerOperation("Gets offset paginated orders per customer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for orders for specified customer Id.", typeof(ApiResponse<PagedResult<OrderCustomerBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpGet("/api/v{v:apiVersion}/" + OrdersModule.BasePath + "/customer/{customerId:guid}" + "/[controller]")]
        public async Task<ActionResult<ApiResponse<PagedResult<OrderCustomerBrowseDto>>>> BrowseOrderssByCustomerId([FromRoute] Guid customerId, [FromQuery] BrowseOrdersByCustomerId query,
            CancellationToken cancellationToken)
        {
            query.CustomerId = customerId;
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<OrderCustomerBrowseDto>>(HttpStatusCode.OK, result));
        }

        [SwaggerOperation("Get a specific order")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a specific order by id.", typeof(ApiResponse<OrderDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order was not found")]
        [AllowAnonymous]
        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<ApiResponse<OrderDetailsDto>>> GetOrder([FromRoute]Guid orderId, CancellationToken cancellationToken)
            => OkOrNotFound<OrderDetailsDto, Order>(await _mediator.Send(new GetOrder(orderId), cancellationToken));

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Creates an order")]
        [SwaggerResponse(StatusCodes.Status201Created, "Creates an order and returns it's identifier", typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPost]
        public async Task<ActionResult> CreateDraftOrder(CancellationToken cancellationToken)
        {
            var orderId = await _mediator.Send(new CreateDraftOrder(), cancellationToken);
            return CreatedAtAction(nameof(GetOrder), new { orderId }, orderId);
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Submits an order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPut("{orderId:guid}/submit-order")]
        public async Task<ActionResult> SubmitOrder([FromRoute] Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SubmitOrder(orderId), cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Adds product to a specified order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPatch("{orderId:guid}/products")]
        public async Task<ActionResult> AddProduct([FromRoute]Guid orderId, [FromBody]AddProduct command, CancellationToken cancellationToken)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Deletes product from a specified order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpDelete("{orderId:guid}/products")]
        public async Task<ActionResult> RemoveProduct([FromRoute] Guid orderId, [FromBody] RemoveProduct command, CancellationToken cancellationToken)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Updates a products's price for specified order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPatch("{orderId:guid}/products/{productId:int}/unit-price")]
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
        public async Task<ActionResult> CancelOrder([FromRoute] Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CancelOrder(orderId), cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Deletes an order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpDelete("{orderId:guid}")]
        public async Task<ActionResult> DeleteOrder([FromRoute]Guid orderId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteOrder(orderId));
            return NoContent();
        }

        [SwaggerOperation("Returns an order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [AllowAnonymous]
        [HttpPost("{orderId:guid}/return")]
        public async Task<ActionResult> ReturnOrder([FromRoute] Guid orderId, [FromBody]ReturnOrder command, CancellationToken cancellationToken)
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
        public async Task<ActionResult> SubmitComplaint([FromRoute] Guid orderId, [FromForm]SubmitComplaint command, CancellationToken cancellationToken)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Updates a order's additional information")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPatch("{orderId:guid}/additional-information")]
        public async Task<ActionResult> WriteAdditionalInformation([FromRoute]Guid orderId, [FromBody]string additionalInformation, 
            CancellationToken cancellationToken)
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
