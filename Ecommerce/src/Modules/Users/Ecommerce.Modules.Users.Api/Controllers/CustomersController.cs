using Asp.Versioning;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Api.Controllers
{
    [Authorize]
    [ApiVersion(1)]
    internal class CustomersController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [SwaggerOperation("Gets offset paginated customers")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for customers.", typeof(ApiResponse<PagedResult<CustomerBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [Authorize(Roles = "Admin, Manager, Employee")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<CustomerBrowseDto>>>> BrowseCustomers([FromQuery] SieveModel model)
            => PagedResult(await _customerService.BrowseAsync(model));

        [SwaggerOperation("Get a specific customer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a specific customer by id.", typeof(ApiResponse<CustomerDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer was not found")]
        [HttpGet("{customerId:guid}")]
        public async Task<ActionResult<ApiResponse<CustomerDetailsDto>>> GetCustomer([FromRoute] Guid customerId)
            => OkOrNotFound(await _customerService.GetAsync(customerId), nameof(Customer));

        [SwaggerOperation("Deletes a customer")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [Authorize(Roles = "Admin, Manager, Employee")]
        [HttpDelete("{customerId:guid}")]
        public async Task<ActionResult> DeleteCustomer([FromRoute] Guid customerId)
        {
            await _customerService.DeleteAsync(customerId);
            return NoContent();
        }

        [SwaggerOperation("Updates a customer")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{customerId:guid}")]
        public async Task<ActionResult> UpdateCustomer([FromRoute] Guid customerId, [FromBody] CustomerUpdateDto dto)
        {
            dto.CustomerId = customerId;
            await _customerService.UpdateAsync(dto);
            return NoContent();
        }

        [SwaggerOperation("Activates a customer")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [Authorize(Roles = "Admin, Manager, Employee")]
        [HttpPut("{customerId:guid}/activate")]
        public async Task<ActionResult> ActivateCustomer([FromRoute] Guid customerId)
        {
            await _customerService.SetActiveAsync(customerId, true);
            return NoContent();
        }

        [SwaggerOperation("Deactivates a customer")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{customerId:guid}/deactivate")]
        public async Task<ActionResult> DeactivateCustomer([FromRoute] Guid customerId)
        {
            await _customerService.SetActiveAsync(customerId, false);
            return NoContent();
        }
    }
}
