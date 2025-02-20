using Asp.Versioning;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.BrowseManufacturers;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.ChangeManufacturerName;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.CreateManufacturer;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.DeleteManufacturer;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.DeleteSelectedManufacturers;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.GetManufacturer;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
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
    internal class ManufacturerController : BaseController
    {
        public ManufacturerController(IMediator mediator) : base(mediator)
        {
        }
        //[HttpGet("{id:guid}")]
        //public async Task<ActionResult<ApiResponse<ManufacturerBrowseDto>>> GetManufacturer([FromRoute] Guid id, CancellationToken cancellationToken = default)
        //    => OkOrNotFound<ManufacturerBrowseDto, Manufacturer>(await _mediator.Send(new GetManufacturer(id), cancellationToken));
        [HttpPost()]
        public async Task<ActionResult> CreateManufacturer([FromBody]CreateManufacturer command, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command, cancellationToken);
            return Created();
        }
        [HttpGet()]
        public async Task<ActionResult<ApiResponse<PagedResult<ManufacturerBrowseDto>>>> BrowseManufacturers([FromQuery] BrowseManufacturers query,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<ManufacturerBrowseDto>>(HttpStatusCode.OK, result));
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteManufacturer([FromRoute]Guid id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteManufacturer(id), cancellationToken);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> ChangeManufacturerName([FromRoute]Guid id, [FromBody]ChangeManufacturerName command, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command with
            {
                ManufaturerId = id
            });
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedManufacturers([FromBody]DeleteSelectedManufacturers command, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

    }
}
