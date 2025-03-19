using Asp.Versioning;
using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
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

        [SwaggerOperation("Creates a manufacturer")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [HttpPost()]
        public async Task<ActionResult> CreateManufacturer([FromBody]CreateManufacturer command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Created();
        }

        [SwaggerOperation("Gets offset paginated manufacturers")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for manufacturers.", typeof(ApiResponse<PagedResult<ManufacturerBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet()]
        public async Task<ActionResult<ApiResponse<PagedResult<ManufacturerBrowseDto>>>> BrowseManufacturers([FromQuery] BrowseManufacturers query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<ManufacturerBrowseDto>>(HttpStatusCode.OK, result));
        }

        [SwaggerOperation("Deletes a manufacturer")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteManufacturer([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteManufacturer(id), cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Updates a manufacturer's name")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult> ChangeManufacturerName([FromRoute]Guid id, [FromBody]ChangeManufacturerName command, 
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command with
            {
                ManufaturerId = id
            }, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Deletes many manufacturers")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedManufacturers([FromBody]DeleteSelectedManufacturers command, 
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

    }
}
