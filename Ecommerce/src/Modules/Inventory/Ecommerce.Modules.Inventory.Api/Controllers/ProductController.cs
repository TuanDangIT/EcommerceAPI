using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.ChangeManufacturerName;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.CreateManufacturer;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteManufacturer;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteSelectedManufacturers;
using Ecommerce.Modules.Inventory.Application.Features.Products.BrowseProducts;
using Ecommerce.Modules.Inventory.Application.Features.Products.CreateProduct;
using Ecommerce.Modules.Inventory.Application.Features.Products.DeleteProduct;
using Ecommerce.Modules.Inventory.Application.Features.Products.DeleteSelectedProducts;
using Ecommerce.Modules.Inventory.Application.Features.Products.UpdateProduct;
using Ecommerce.Shared.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpGet()]
        public async Task<ActionResult<PagedResult<ProductListingDto>>> BrowseProducts([FromBody]BrowseProducts query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
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
