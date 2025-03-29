using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.BrowseShipments;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe.Entitlements;
using Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.DeleteShipment;
using Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.DownloadLabel;
using Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.CreateShipment;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    [Route("/api/v{v:apiVersion}/" + OrdersModule.BasePath + "/orders/{orderId:guid}/[controller]")]
    internal class ShipmentsController : BaseController
    {
        public ShipmentsController(IMediator mediator) : base(mediator)
        {
        }

        [SwaggerOperation("Gets cursor paginated shipments")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns cursor paginated result for shipments.", typeof(ApiResponse<CursorPagedResult<ShipmentBrowseDto, ShipmentCursorDto>>))]
        [HttpGet("/api/v{v:apiVersion}/" + OrdersModule.BasePath + "/[controller]")]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<ShipmentBrowseDto, ShipmentCursorDto>>>> BrowseShipments([FromQuery] BrowseShipments query, 
            CancellationToken cancellationToken)
            => CursorPagedResult(await _mediator.Send(query, cancellationToken));

        [SwaggerOperation("Creates an shipment for a specified order")]
        [SwaggerResponse(StatusCodes.Status201Created, "Creates a manufacturer for specified order and returns it's identifier.", typeof(int))]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> CreateShipment([FromRoute] Guid orderId, [FromBody] CreateShipment command, CancellationToken cancellationToken)
        {
            command = command with { OrderId = orderId };
            var shipmentId = await _mediator.Send(command, cancellationToken);
            Response.Headers.Append("shipment-id", shipmentId.ToString());
            return Created(default(string), new ApiResponse<object>(System.Net.HttpStatusCode.Created, new { Id = shipmentId }));
        }

        [SwaggerOperation("Downloads a label")]
        [SwaggerResponse(StatusCodes.Status200OK, "Downloads an label for a specified order.", typeof(ApiResponse<IFormFile>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpGet("{shipmentId:int}")]
        public async Task<ActionResult<IFormFile>> DownloadLabel([FromRoute] Guid orderId, [FromRoute] int shipmentId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DownloadLabel(orderId, shipmentId), cancellationToken);
            return File(result.FileStream, result.MimeType, result.FileName);
        }

        [SwaggerOperation("Deletes an shipment from a specified order")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpDelete("{shipmentId:int}")]
        public async Task<ActionResult> DeleteShipment([FromRoute] Guid orderId, [FromRoute] int shipmentId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteShipment(orderId, shipmentId), cancellationToken);
            return NoContent();
        }
    }
}
