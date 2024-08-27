using Ecommerce.Modules.Products.Core.DTO;
using Ecommerce.Modules.Products.Core.Entities;
using Ecommerce.Modules.Products.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Api.Controllers
{
    internal class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ProductBrowseDto>>>> BrowseProducts([FromBody]SieveModel sieveModel)
            => Ok(new ApiResponse<PagedResult<ProductBrowseDto>>(HttpStatusCode.OK, "success", await _productService.BrowseProductsAsync(sieveModel)));
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ProductDetailsDto>>> GetProduct([FromRoute] Guid id)
            => OkOrNotFound<ProductDetailsDto, Product>(await _productService.GetAsync(id));
    }
}
