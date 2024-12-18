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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    [ApiVersion(1)]
    internal class ProductController : BaseController
    {
        public ProductController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost()]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProduct command)
        {
            var productId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProduct), new {productId}, null);
        }
        [HttpPost("import")]
        public async Task<ActionResult> ImportProducts([FromForm] ImportProducts command)
        {
            await _mediator.Send(command);
            return Created();
        }
        [HttpGet()]
        public async Task<ActionResult<ApiResponse<PagedResult<ProductBrowseDto>>>> BrowseProducts([FromQuery] BrowseProducts query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<PagedResult<ProductBrowseDto>>(HttpStatusCode.OK, result));
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ProductDetailsDto>>> GetProduct([FromRoute]Guid id)
            => OkOrNotFound<ProductDetailsDto, Product>(await _mediator.Send(new GetProduct(id)));
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteProduct(id));
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteSelectedProducts([FromBody] DeleteSelectedProducts command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateProduct([FromForm] UpdateProduct command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpPost("list")]
        public async Task<ActionResult> ListProduct([FromBody]Guid[] ids)
        {
            await _mediator.Send(new ListProducts(ids));
            return NoContent();
        }
        [HttpPost("unlist")]
        public async Task<ActionResult> UnlistProduct([FromBody] Guid[] ids)
        {
            await _mediator.Send(new UnlistProducts(ids));
            return NoContent();
        }
        [HttpPut("{productId:guid}/quantity")]
        public async Task<ActionResult> ChangeProductQuantity([FromRoute]Guid productId, [FromBody]int quantity)
        {
            await _mediator.Send(new ChangeProductQuantity(productId, quantity));
            return NoContent();
        }
        [HttpPut("{productId:guid}/price")]
        public async Task<ActionResult> ChangeProductPrice([FromRoute] Guid productId, [FromBody] decimal price)
        {
            await _mediator.Send(new ChangeProductPrice(productId, price));
            return NoContent();
        }
        [HttpPut("{productId:guid}/reserved")]
        public async Task<ActionResult> ChangeProductReservedQuantity([FromRoute] Guid productId, [FromBody] int reserved)
        {
            await _mediator.Send(new ChangeProductReservedQuantity(productId, reserved));
            return NoContent();
        }
    }
}
