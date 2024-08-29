using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.BrowseProducts;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.CreateProduct;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.DeleteProduct;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.DeleteSelectedProducts;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.GetProduct;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ListProduct;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.UnlistProduct;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.UpdateProduct;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
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
        [HttpGet()]
        public async Task<ActionResult<ApiResponse<PagedResult<ProductBrowseDto>>>> BrowseProducts([FromQuery] BrowseProducts query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<PagedResult<ProductBrowseDto>>(HttpStatusCode.OK, "success", result));
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
            await _mediator.Send(new ListProduct(ids));
            return NoContent();
        }
        [HttpPost("unlist")]
        public async Task<ActionResult> UnlistProduct([FromBody] Guid[] ids)
        {
            await _mediator.Send(new UnlistProduct(ids));
            return NoContent();
        }
    }
}
