﻿using Asp.Versioning;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.BrowseManufacturers;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.ChangeManufacturerName;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.CreateManufacturer;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.DeleteManufacturer;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.DeleteSelectedManufacturers;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.GetManufacturer;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
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
    internal class ManufacturerController : BaseController
    {
        public ManufacturerController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ManufacturerBrowseDto>>> GetManufacturer([FromRoute] Guid id)
            => OkOrNotFound<ManufacturerBrowseDto, Manufacturer>(await _mediator.Send(new GetManufacturer(id)));
        [HttpPost()]
        public async Task<ActionResult> CreateManufacturer([FromBody]CreateManufacturer command)
        {
            await _mediator.Send(command);
            return Created();
        }
        [HttpGet()]
        public async Task<ActionResult<ApiResponse<PagedResult<ManufacturerBrowseDto>>>> BrowseManufacturers([FromQuery] BrowseManufacturers query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<PagedResult<ManufacturerBrowseDto>>(HttpStatusCode.OK, result));
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
