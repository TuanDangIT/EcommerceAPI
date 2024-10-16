using Ecommerce.Modules.Orders.Application.Shipping.DTO;
using Ecommerce.Modules.Orders.Application.Shipping.Features.BrowseShippings;
using Ecommerce.Modules.Orders.Application.Shipping.Features.DownloadLabel;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    internal class ShipmentController : BaseController
    {
        public ShipmentController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<ShipmentBrowseDto, ShipmentCursorDto>>>> BrowseShipments([FromQuery] BrowseShipments query)
            => CursorPagedResult(await _mediator.Send(query));
        [HttpGet("{shipmentId:int}")]
        public async Task<ActionResult<IFormFile>> DownloadLabel([FromRoute]int shipmentId)
        {
            var result = await _mediator.Send(new DownloadLabel(shipmentId));
            return File(result.FileStream, result.MimeType, result.FileName);
        }
    }
}
