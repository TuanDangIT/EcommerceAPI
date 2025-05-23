﻿using Asp.Versioning;
using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.BrowseParameters;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.ChangeParameterName;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.CreateParameter;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.DeleteParameter;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.DeleteSelectedParameters;
using Ecommerce.Shared.Abstractions.Api;
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

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class ParametersController : BaseController
    {
        public ParametersController(IMediator mediator) : base(mediator)
        {
        }

        [SwaggerOperation("Creates a paramter")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> CreateParameter([FromBody] CreateParameter command, CancellationToken cancellationToken)
        {
            var parameterId = await _mediator.Send(command, cancellationToken);
            Response.Headers.Append("parameter-id", parameterId.ToString());
            return Created(parameterId.ToString());
        }

        [SwaggerOperation("Gets offset paginated parameters")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for parameters.", typeof(ApiResponse<PagedResult<ParameterBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ParameterBrowseDto>>>> BrowseParameters([FromQuery] BrowseParameters query, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<ParameterBrowseDto>>(HttpStatusCode.OK, result));
        }

        [SwaggerOperation("Updates a parameter's name")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult> ChangeParameterName([FromRoute] Guid id, [FromBody] ChangeParameterName command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command with
            {
                ParameterId = id
            }, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Deletes a parameter")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteParameter([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteParameter(id), cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Deletes many parameters")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedParameters([FromBody] DeleteSelectedParameters command, 
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
