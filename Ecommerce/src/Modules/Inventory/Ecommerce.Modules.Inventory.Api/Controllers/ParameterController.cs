using Asp.Versioning;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.BrowseParameters;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.ChangeParameterName;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.CreateParameter;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.DeleteParameter;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.DeleteSelectedParameters;
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

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
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
        public async Task<ActionResult<ApiResponse<PagedResult<ParameterBrowseDto>>>> BrowseParameters([FromQuery] BrowseParameters query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<PagedResult<ParameterBrowseDto>>(HttpStatusCode.OK, result));
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteParameter([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteParameter(id));
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> ChangeParameterName([FromRoute] Guid id, [FromBody] ChangeParameterName command)
        {
            await _mediator.Send(command with
            {
                ParameterId = id
            });
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedParameters([FromBody] DeleteSelectedParameters command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
