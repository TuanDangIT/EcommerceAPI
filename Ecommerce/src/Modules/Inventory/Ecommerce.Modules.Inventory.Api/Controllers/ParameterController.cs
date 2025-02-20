using Asp.Versioning;
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
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class ParameterController : BaseController
    {
        public ParameterController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost]
        public async Task<ActionResult> CreateParameter([FromBody] CreateParameter command)
        {
            await _mediator.Send(command);
            return Created();
        }
        [HttpGet()]
        public async Task<ActionResult<ApiResponse<PagedResult<ParameterBrowseDto>>>> BrowseParameters([FromQuery] BrowseParameters query, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<ParameterBrowseDto>>(HttpStatusCode.OK, result));
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteParameter([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteParameter(id), cancellationToken);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> ChangeParameterName([FromRoute] Guid id, [FromBody] ChangeParameterName command,
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command with
            {
                ParameterId = id
            }, cancellationToken);
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedParameters([FromBody] DeleteSelectedParameters command, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
