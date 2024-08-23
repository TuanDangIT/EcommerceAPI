﻿using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Category.BrowseCategory;
using Ecommerce.Modules.Inventory.Application.Features.Category.ChangeCategoryName;
using Ecommerce.Modules.Inventory.Application.Features.Category.CreateCategory;
using Ecommerce.Modules.Inventory.Application.Features.Category.DeleteCategory;
using Ecommerce.Modules.Inventory.Application.Features.Category.DeleteSelectedCategories;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.ChangeManufacturerName;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.CreateManufacturer;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteManufacturer;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteSelectedManufacturers;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.GetManufacturer;
using Ecommerce.Modules.Inventory.Application.Features.Products.BrowseProducts;
using Ecommerce.Shared.Abstractions.ApiResponse;
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
    internal class CategoryController : BaseController
    {
        public CategoryController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost()]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategory command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpGet()]
        public async Task<ActionResult<PagedResult<CategoryBrowseDto>>> BrowseCategories([FromQuery] BrowseCategories query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteCategory(id));
            return Ok(new ApiResponse(200, "success"));
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> ChangeCategoryName([FromRoute] Guid id, [FromBody] ChangeCategoryName command)
        {
            await _mediator.Send(command with
            {
                CategoryId = id
            });
            return Ok(new ApiResponse(200, "success"));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedCategories([FromBody] DeleteSelectedCategories command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
