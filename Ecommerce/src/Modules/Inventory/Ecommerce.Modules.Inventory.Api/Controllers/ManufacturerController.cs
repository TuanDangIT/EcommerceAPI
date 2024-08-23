using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Category.BrowseCategory;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.BrowseManufacturers;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.ChangeManufacturerName;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.CreateManufacturer;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteManufacturer;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteSelectedManufacturers;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.GetManufacturer;
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
    internal class ManufacturerController : BaseController
    {
        public ManufacturerController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ManufacturerBrowseDto?>> GetManufacturer([FromRoute] Guid id)
            => OkOrNotFound(await _mediator.Send(new GetManufacturer(id)));
        [HttpPost()]
        public async Task<ActionResult> CreateManufacturer([FromBody]CreateManufacturer command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpGet()]
        public async Task<ActionResult<PagedResult<ManufacturerBrowseDto>>> BrowseManufacturers([FromQuery] BrowseManufacturers query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteManufacturer([FromRoute]Guid id)
        {
            await _mediator.Send(new DeleteManufacturer(id));
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> ChangeManufacturerName([FromRoute]Guid id, [FromBody]ChangeManufacturerName command)
        {
            await _mediator.Send(command with
            {
                ManufaturerId = id
            });
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedManufacturers([FromBody]DeleteSelectedManufacturers command)
        {
            await _mediator.Send(command);
            return NoContent(); 
        }

    }
}
