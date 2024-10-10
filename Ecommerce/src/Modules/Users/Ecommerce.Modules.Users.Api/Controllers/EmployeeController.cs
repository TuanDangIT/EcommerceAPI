using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Api.Controllers
{
    internal class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private const string EmployeeEntityName = "Employee";

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpPost]
        public async Task<ActionResult> CreateEmployee([FromBody] EmployeeCreateDto dto)
        {
            var employeeId = await _employeeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetEmployee), new { employeeId }, null);
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeBrowseDto>>>> BrowseEmployees([FromQuery] SieveModel model)
            => PagedResult(await _employeeService.BrowseAsync(model));
        [HttpGet("{employeeId:guid}")]
        public async Task<ActionResult<ApiResponse<EmployeeDetailsDto>>> GetEmployee([FromRoute] Guid employeeId)
            => OkOrNotFound(await _employeeService.GetAsync(employeeId), EmployeeEntityName);
        [HttpDelete("{employeeId:guid}")]
        public async Task<ActionResult> DeleteEmployee([FromRoute]Guid employeeId)
        {
            await _employeeService.DeleteAsync(employeeId);
            return NoContent();
        }
        [HttpPut("{employeeId:guid}")]
        public async Task<ActionResult> UpdateEmployee([FromRoute] Guid employeeId, [FromBody] EmployeeUpdateDto dto)
        {
            dto.EmployeeId = employeeId;
            await _employeeService.UpdateAsync(dto);
            return NoContent();
        }
        [HttpPost("{employeeId:guid}/activate")]
        public async Task<ActionResult> ActivateCustomer([FromRoute] Guid employeeId)
        {
            await _employeeService.SetActiveAsync(employeeId, true);
            return NoContent();
        }
        [HttpPost("{employeeId:guid}/deactivate")]
        public async Task<ActionResult> DeactivateCustomer([FromRoute] Guid employeeId)
        {
            await _employeeService.SetActiveAsync(employeeId, false);
            return NoContent();
        }
    }
}
