using Ecommerce.Modules.Inventory.Application.DTO;
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
    internal class CategoryController : BaseController
    {
        public CategoryController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost()]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategory command)
        {
            await _mediator.Send(command);
            return Created();
        }
        [HttpGet()]
        public async Task<ActionResult<ApiResponse<PagedResult<CategoryBrowseDto>>>> BrowseCategories([FromQuery] BrowseCategories query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<PagedResult<CategoryBrowseDto>>(HttpStatusCode.OK, "success", result));
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteCategory(id));
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> ChangeCategoryName([FromRoute] Guid id, [FromBody] ChangeCategoryName command)
        {
            await _mediator.Send(command with
            {
                CategoryId = id
            });
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedCategories([FromBody] DeleteSelectedCategories command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
