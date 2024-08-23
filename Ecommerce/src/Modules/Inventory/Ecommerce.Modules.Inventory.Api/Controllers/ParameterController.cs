using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Category.BrowseCategory;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteSelectedManufacturers;
using Ecommerce.Modules.Inventory.Application.Features.Parameters;
using Ecommerce.Modules.Inventory.Application.Features.Parameters.BrowseParameters;
using Ecommerce.Modules.Inventory.Application.Features.Parameters.ChangeParameterName;
using Ecommerce.Modules.Inventory.Application.Features.Parameters.CreateParameter;
using Ecommerce.Modules.Inventory.Application.Features.Parameters.DeleteParameter;
using Ecommerce.Modules.Inventory.Application.Features.Parameters.DeleteSelectedParameters;
using Ecommerce.Shared.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    internal class ParameterController : BaseController
    {
        public ParameterController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost]
        public async Task<ActionResult> CreateParameter([FromBody] CreateParameter command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpGet()]
        public async Task<ActionResult<PagedResult<ParameterBrowseDto>>> BrowseParameters([FromQuery] BrowseParameters query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
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
