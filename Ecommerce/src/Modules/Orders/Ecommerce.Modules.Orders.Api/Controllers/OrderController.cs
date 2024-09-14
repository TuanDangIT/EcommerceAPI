using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.GetOrder;
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
        public async Task<ActionResult<ApiResponse<CursorPagedResult<OrderBrowseDto, CursorDto>>>> BrowseOrders([FromQuery] BrowseOrders query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<CursorPagedResult<OrderBrowseDto, CursorDto>>(HttpStatusCode.OK, result));
        }
        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<ApiResponse<OrderDetailsDto>>> GetOrder([FromRoute]Guid orderId)
        {
            var result = await _mediator.Send(new GetOrder(orderId));
            return Ok(new ApiResponse<OrderDetailsDto>(HttpStatusCode.OK, result));
        }
    }
}
