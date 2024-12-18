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

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    [ApiVersion(1)]
    [Route("/api/v{v:apiVersion}/" + OrdersModule.BasePath + "/orders/{orderId:guid}/[controller]")]
    internal class ShipmentsController : BaseController
    {
        public ShipmentsController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("/api/v{v:apiVersion}/" + OrdersModule.BasePath + "/[controller]")]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<ShipmentBrowseDto, ShipmentCursorDto>>>> BrowseShipments([FromQuery] BrowseShipments query, 
            CancellationToken cancellationToken = default)
            => CursorPagedResult(await _mediator.Send(query, cancellationToken));
        [HttpPost()]
        public async Task<ActionResult> CreateShipment([FromRoute] Guid orderId, [FromBody] CreateShipment command, CancellationToken cancellationToken = default)
        {
            command = command with { OrderId = orderId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        [HttpGet("{shipmentId:int}")]
        public async Task<ActionResult<IFormFile>> DownloadLabel([FromRoute] Guid orderId, [FromRoute] int shipmentId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DownloadLabel(orderId, shipmentId), cancellationToken);
            return File(result.FileStream, result.MimeType, result.FileName);
        }
        [HttpDelete("{shipmentId:int}")]
        public async Task<ActionResult> DeleteShipment([FromRoute] Guid orderId, [FromRoute] int shipmentId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteShipment(orderId, shipmentId), cancellationToken);
            return NoContent();
        }
    }
}
