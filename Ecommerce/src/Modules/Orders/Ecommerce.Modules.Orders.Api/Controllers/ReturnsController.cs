﻿using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.BrowseReturns;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.GetReturn;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.HandleReturn;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.RejectReturn;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.SetNote;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
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
    [ApiVersion(1)]
    internal class ReturnsController : BaseController
    {
        public ReturnsController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>>>> BrowseOrders([FromQuery] BrowseReturns query, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>>(HttpStatusCode.OK, result));
        }
        [HttpGet("{returnId:guid}")]
        public async Task<ActionResult<ApiResponse<ReturnDetailsDto>>> GetOrder([FromRoute] Guid returnId, CancellationToken cancellationToken = default)
            => OkOrNotFound<ReturnDetailsDto, Return>(await _mediator.Send(new GetReturn(returnId), cancellationToken));
        [HttpPost("{returnId:guid}/handle")]
        public async Task<ActionResult> HandleReturn([FromRoute]Guid returnId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new HandleReturn(returnId), cancellationToken);
            return NoContent();
        }
        [HttpPost("{returnId:guid}/reject")]
        public async Task<ActionResult> RejectReturn([FromRoute] Guid returnId, [FromBody] RejectReturn command, CancellationToken cancellationToken = default)
        {
            command = command with { ReturnId = returnId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        [HttpPut("{returnId:guid}/note")]
        public async Task<ActionResult> SetNote([FromRoute] Guid returnId, [FromForm]string note, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new SetNote(note, returnId), cancellationToken);
            return NoContent();
        }
    }
}
