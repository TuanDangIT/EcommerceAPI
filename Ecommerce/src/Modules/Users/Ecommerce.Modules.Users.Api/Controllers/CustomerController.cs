using Asp.Versioning;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Api.Controllers
{
    [ApiVersion(1)]
    internal class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        private const string CustomerEntityName = "Customer";

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<CustomerBrowseDto>>>> BrowseCustomers([FromQuery] SieveModel model)
            => PagedResult(await _customerService.BrowseAsync(model));
        [HttpGet("{customerId:guid}")]
        public async Task<ActionResult<ApiResponse<CustomerDetailsDto>>> GetCustomer([FromRoute] Guid customerId)
            => OkOrNotFound(await _customerService.GetAsync(customerId), CustomerEntityName);
        [HttpDelete("{customerId:guid}")]
        public async Task<ActionResult> DeleteCustomer([FromRoute] Guid customerId)
        {
            await _customerService.DeleteAsync(customerId);
            return NoContent();
        }
        [HttpPut("{customerId:guid}")]
        public async Task<ActionResult> UpdateCustomer([FromRoute] Guid customerId, [FromBody] CustomerUpdateDto dto)
        {
            dto.CustomerId = customerId;
            await _customerService.UpdateAsync(dto);
            return NoContent();
        }
        [HttpPost("{customerId:guid}/activate")]
        public async Task<ActionResult> ActivateCustomer([FromRoute] Guid customerId)
        {
            await _customerService.SetActiveAsync(customerId, true);
            return NoContent();
        }
        [HttpPost("{customerId:guid}/deactivate")]
        public async Task<ActionResult> DeactivateCustomer([FromRoute] Guid customerId)
        {
            await _customerService.SetActiveAsync(customerId, false);
            return NoContent();
        }
    }
}
