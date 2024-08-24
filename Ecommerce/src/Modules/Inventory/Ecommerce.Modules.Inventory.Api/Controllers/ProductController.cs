using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.ChangeManufacturerName;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.CreateManufacturer;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteManufacturer;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteSelectedManufacturers;
using Ecommerce.Modules.Inventory.Application.Features.Products.BrowseProducts;
using Ecommerce.Modules.Inventory.Application.Features.Products.CreateProduct;
using Ecommerce.Modules.Inventory.Application.Features.Products.DeleteProduct;
using Ecommerce.Modules.Inventory.Application.Features.Products.DeleteSelectedProducts;
using Ecommerce.Modules.Inventory.Application.Features.Products.GetProduct;
using Ecommerce.Modules.Inventory.Application.Features.Products.UpdateProduct;
using Ecommerce.Modules.Inventory.Domain.Entities;
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
        public async Task<ActionResult<ApiResponse<PagedResult<ProductListingDto>>>> BrowseProducts([FromQuery] BrowseProducts query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<PagedResult<ProductListingDto>>(HttpStatusCode.OK, "success", result));
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
    }
}
