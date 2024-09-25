using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.CancelOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.GetOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.ReturnOrder;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.SubmitComplaint;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
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
    }
}
