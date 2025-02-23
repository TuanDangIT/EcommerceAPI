using Asp.Versioning;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.BrowseCategories;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.ChangeCategoryName;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.CreateCategory;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.DeleteCategory;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.DeleteSelectedCategories;
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
    internal class CategoriesController : BaseController
    {
        public CategoriesController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategory command)
        {
            await _mediator.Send(command);
            return Created();
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<CategoryBrowseDto>>>> BrowseCategories([FromQuery] BrowseCategories query, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<CategoryBrowseDto>>(HttpStatusCode.OK, result));
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteCategory(id), cancellationToken);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> ChangeCategoryName([FromRoute] Guid id, [FromBody] ChangeCategoryName command,
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command with
            {
                CategoryId = id
            }, cancellationToken);
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedCategories([FromBody] DeleteSelectedCategories command, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
