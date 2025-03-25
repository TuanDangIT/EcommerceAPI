using Asp.Versioning;
using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
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

    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class CategoriesController : BaseController
    {
        public CategoriesController(IMediator mediator) : base(mediator)
        {
        }

        [SwaggerOperation("Creates a category")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategory command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Created();
        }

        [SwaggerOperation("Gets offset paginated categories")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for categories.", typeof(ApiResponse<PagedResult<CategoryBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<CategoryBrowseDto>>>> BrowseCategories([FromQuery] BrowseCategories query, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<CategoryBrowseDto>>(HttpStatusCode.OK, result));
        }

        [SwaggerOperation("Deletes a category")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteCategory(id), cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Updates a category's name")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult> ChangeCategoryName([FromRoute] Guid id, [FromBody] ChangeCategoryName command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command with
            {
                CategoryId = id
            }, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Deletes many categories")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedCategories([FromBody] DeleteSelectedCategories command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
