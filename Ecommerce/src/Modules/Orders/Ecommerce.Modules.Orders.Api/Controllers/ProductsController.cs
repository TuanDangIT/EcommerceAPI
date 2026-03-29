using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.BrowseProductsToAdd;
using Ecommerce.Shared.Abstractions.Api;
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
    internal class ProductsController : BaseController
    {
        public ProductsController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Browse available products to add to order")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns result for products available to add to order from search filter.", typeof(ApiResponse<IEnumerable<ProductToAddBrowseDto>>))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductToAddBrowseDto>>>> BrowseProductsToAdd([FromRoute] Guid orderId, [FromQuery] string search, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new BrowseProductsToAdd(orderId, search), cancellationToken);
            return Ok(new ApiResponse<IEnumerable<ProductToAddBrowseDto>>(HttpStatusCode.OK, result));
        }
    }
}
