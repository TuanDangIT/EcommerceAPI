using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.AddProduct;
using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.BrowseReturns;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.GetReturn;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.HandleReturn;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.RejectReturn;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.SetNote;
using Ecommerce.Modules.Orders.Application.Returns.Features.Returns.AddProductToReturn;
using Ecommerce.Modules.Orders.Application.Returns.Features.Returns.BrowseReturnsByCustomerId;
using Ecommerce.Modules.Orders.Application.Returns.Features.Returns.DeleteReturn;
using Ecommerce.Modules.Orders.Application.Returns.Features.Returns.RemoveReturnProduct;
using Ecommerce.Modules.Orders.Application.Returns.Features.Returns.SetReturnProductQuantity;
using Ecommerce.Modules.Orders.Application.Returns.Features.Returns.SetReturnProductStatus;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
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

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    [ApiVersion(1)]
    internal class ReturnsController : BaseController
    {
        public ReturnsController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Gets cursor paginated returns")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns cursor paginated result for returns.", typeof(ApiResponse<CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>>))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>>>> BrowseReturns([FromQuery] BrowseReturns query, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<CursorPagedResult<ReturnBrowseDto, ReturnCursorDto>>(HttpStatusCode.OK, result));
        }

        [Authorize(Roles = "Customer")]
        [SwaggerOperation("Gets offset paginated returns per customer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for returns for specified customer Id.", typeof(ApiResponse<PagedResult<ReturnBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpGet("/api/v{v:apiVersion}/" + OrdersModule.BasePath + "/customer/{customerId:guid}" + "/[controller]")]
        public async Task<ActionResult<ApiResponse<PagedResult<ReturnBrowseDto>>>> BrowseReturnsByCustomerId([FromRoute] Guid customerId, [FromQuery] BrowseReturnsByCustomerId query,
            CancellationToken cancellationToken)
        {
            query.CustomerId = customerId;
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<ReturnBrowseDto>>(HttpStatusCode.OK, result));
        }

        [SwaggerOperation("Get a specific return")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a specific return by id.", typeof(ApiResponse<ReturnDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Return was not found")]
        [AllowAnonymous]
        [HttpGet("{returnId:guid}")]
        public async Task<ActionResult<ApiResponse<ReturnDetailsDto>>> GetReturn([FromRoute] Guid returnId, CancellationToken cancellationToken)
            => OkOrNotFound<ReturnDetailsDto, Return>(await _mediator.Send(new GetReturn(returnId), cancellationToken));

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Deletes a return")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpDelete("{returnId:guid}")]
        public async Task<ActionResult<ApiResponse<ReturnDetailsDto>>> DeleteReturn([FromRoute] Guid returnId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteReturn(returnId), cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Handles a return")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPut("{returnId:guid}/handle")]
        public async Task<ActionResult> HandleReturn([FromRoute]Guid returnId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new HandleReturn(returnId), cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Rejects a return")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPut("{returnId:guid}/reject")]
        public async Task<ActionResult> RejectReturn([FromRoute] Guid returnId, [FromBody] RejectReturn command, CancellationToken cancellationToken)
        {
            command = command with { ReturnId = returnId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Updates a return's note")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPatch("{returnId:guid}/note")]
        public async Task<ActionResult> SetNote([FromRoute] Guid returnId, [FromForm]string note, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SetNote(note, returnId), cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Deletes a product from a specified return")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpDelete("{returnId:guid}/return-products/{productId:int}")]
        public async Task<ActionResult> DeleteReturnProduct([FromRoute] Guid returnId, [FromRoute] int productId,CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemoveReturnProduct(returnId, productId), cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Adds a product to a specified return")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPatch("{returnId:guid}/return-products")]
        public async Task<ActionResult> AddReturnProduct([FromRoute] Guid returnId, [FromBody] AddProductToReturn command, CancellationToken cancellationToken)
        {
            command = command with { ReturnId = returnId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Updates a product's quantity for a specified return")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPatch("{returnId:guid}/return-products/{productId:int}/quantity")]
        public async Task<ActionResult> SetReturnProductQuantity([FromRoute] Guid returnId, [FromRoute] int productId,
            [FromBody] int quantity, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SetReturnProductQuantity(returnId, productId, quantity), cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Updates a product's status for a specified return", "Checks product status if it's correct.")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPatch("{returnId:guid}/return-products/{productId:int}/status")]
        public async Task<ActionResult> SetReturnProductStatus([FromRoute] Guid returnId, [FromRoute] int productId, 
            [FromBody] string status, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SetReturnProductStatus(returnId, productId, status), cancellationToken);
            return NoContent();
        }
    }
}
