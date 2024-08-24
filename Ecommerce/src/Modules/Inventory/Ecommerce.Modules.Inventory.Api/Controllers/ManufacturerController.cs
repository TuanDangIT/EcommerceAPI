﻿using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Category.BrowseCategory;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.BrowseManufacturers;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.ChangeManufacturerName;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.CreateManufacturer;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteManufacturer;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteSelectedManufacturers;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.GetManufacturer;
using Ecommerce.Modules.Inventory.Domain.Entities;
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
    internal class ManufacturerController : BaseController
    {
        public ManufacturerController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse>> GetManufacturer([FromRoute] Guid id)
            => OkOrNotFound<Manufacturer>(await _mediator.Send(new GetManufacturer(id)));
        [HttpPost()]
        public async Task<ActionResult> CreateManufacturer([FromBody]CreateManufacturer command)
        {
            await _mediator.Send(command);
            return Created();
        }
        [HttpGet()]
        public async Task<ActionResult<ApiResponse>> BrowseManufacturers([FromQuery] BrowseManufacturers query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse(HttpStatusCode.OK, "success", result));
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
