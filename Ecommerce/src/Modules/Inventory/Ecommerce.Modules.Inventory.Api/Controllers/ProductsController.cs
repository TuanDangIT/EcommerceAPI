using Asp.Versioning;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.BrowseProducts;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ChangeProductPrice;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ChangeProductQuantity;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ChangeProductReservedQuantity;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.CreateProduct;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.DeleteProduct;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.DeleteSelectedProducts;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.GetProduct;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ImportProducts;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ListProducts;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.UnlistProducts;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.UpdateProduct;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class ProductsController : BaseController
    {
        public ProductsController(IMediator mediator) : base(mediator)
        {
        }

        [SwaggerOperation("Creates a product")]
        [SwaggerResponse(StatusCodes.Status201Created, "Creates a product and returns it's identifier", typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> CreateProduct([FromForm] CreateProduct command, CancellationToken cancellationToken)
        {
            var productId = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetProduct), new { id = productId }, new ApiResponse<object>(HttpStatusCode.Created, new { Id = productId }));
        }

        [SwaggerOperation("Imports products", "Imports products from a csv file.")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [HttpPost("import")]
        public async Task<ActionResult> ImportProducts([FromForm] ImportProducts command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Created();
        }

        [SwaggerOperation("Gets offset paginated products")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for products.", typeof(ApiResponse<PagedResult<ProductBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ProductBrowseDto>>>> BrowseProducts([FromQuery] BrowseProducts query, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<ProductBrowseDto>>(HttpStatusCode.OK, result));
        }

        [SwaggerOperation("Get a specific product")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a specific product by id.", typeof(ApiResponse<ProductDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product was not found")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ProductDetailsDto>>> GetProduct([FromRoute]Guid id, CancellationToken cancellationToken)
            => OkOrNotFound<ProductDetailsDto, Product>(await _mediator.Send(new GetProduct(id), cancellationToken), id.ToString());

        [SwaggerOperation("Deletes a product")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteProduct(id), cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Deletes many products")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedProducts([FromBody] DeleteSelectedProducts command, 
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Updates a product")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateProduct([FromForm] UpdateProduct command, [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            command = command with { Id = id };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Lists a product for sale", "Lists a product so customers can purchase.")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("list")]
        public async Task<ActionResult> ListProduct([FromBody]Guid[] ids, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ListProducts(ids), cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Unlists product from sale", "Unlists product from sale which makes it not visible for customers.")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("unlist")]
        public async Task<ActionResult> UnlistProduct([FromBody] Guid[] ids, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UnlistProducts(ids), cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Changes a products's quantity")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPatch("{productId:guid}/quantity")]
        public async Task<ActionResult> ChangeProductQuantity([FromRoute]Guid productId, [FromBody]int quantity, 
            CancellationToken cancellationToken)
        {
            await _mediator.Send(new ChangeProductQuantity(productId, quantity), cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Changes a products's price")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPatch("{productId:guid}/price")]
        public async Task<ActionResult> ChangeProductPrice([FromRoute] Guid productId, [FromBody] decimal price, 
            CancellationToken cancellationToken)
        {
            await _mediator.Send(new ChangeProductPrice(productId, price), cancellationToken);
            return NoContent();
        }

        [SwaggerOperation("Changes a products's reserved quantity")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPatch("{productId:guid}/reserved")]
        public async Task<ActionResult> ChangeProductReservedQuantity([FromRoute] Guid productId, [FromBody] int reserved, 
            CancellationToken cancellationToken)
        {
            await _mediator.Send(new ChangeProductReservedQuantity(productId, reserved), cancellationToken);
            return NoContent();
        }
    }
}
