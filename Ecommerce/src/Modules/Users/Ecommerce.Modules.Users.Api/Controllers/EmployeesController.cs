using Asp.Versioning;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Api.Controllers
{
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
    [Authorize(Roles = "Admin, Manager")]
    [ApiVersion(1)]
    internal class EmployeesController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private const string _employeeEntityName = "Employee";

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [SwaggerOperation("Creates an employee")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateEmployee([FromBody] EmployeeCreateDto dto)
        {
            var employeeId = await _employeeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetEmployee), new { employeeId }, null);
        }

        [SwaggerOperation("Gets offset paginated employees")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for employees.", typeof(ApiResponse<PagedResult<EmployeeBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeBrowseDto>>>> BrowseEmployees([FromQuery] SieveModel model)
            => PagedResult(await _employeeService.BrowseAsync(model));

        [SwaggerOperation("Get a specific employee")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a specific employee by id.", typeof(ApiResponse<EmployeeDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Employee was not found")]
        [HttpGet("{employeeId:guid}")]
        public async Task<ActionResult<ApiResponse<EmployeeDetailsDto>>> GetEmployee([FromRoute] Guid employeeId)
            => OkOrNotFound(await _employeeService.GetAsync(employeeId), _employeeEntityName);

        [SwaggerOperation("Deletes an employee")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpDelete("{employeeId:guid}")]
        public async Task<ActionResult> DeleteEmployee([FromRoute]Guid employeeId)
        {
            await _employeeService.DeleteAsync(employeeId);
            return NoContent();
        }

        [SwaggerOperation("Updates an employee")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{employeeId:guid}")]
        public async Task<ActionResult> UpdateEmployee([FromRoute] Guid employeeId, [FromBody] EmployeeUpdateDto dto)
        {
            dto.EmployeeId = employeeId;
            await _employeeService.UpdateAsync(dto);
            return NoContent();
        }

        [SwaggerOperation("Activates an employee")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{employeeId:guid}/activate")]
        public async Task<ActionResult> ActivateEmployee([FromRoute] Guid employeeId)
        {
            await _employeeService.SetActiveAsync(employeeId, true);
            return NoContent();
        }

        [SwaggerOperation("Deactivates an employee")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{employeeId:guid}/deactivate")]
        public async Task<ActionResult> DeactivateEmployee([FromRoute] Guid employeeId)
        {
            await _employeeService.SetActiveAsync(employeeId, false);
            return NoContent();
        }
    }
}
