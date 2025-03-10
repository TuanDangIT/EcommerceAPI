using Asp.Versioning;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Api.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [SwaggerOperation("Gets offset paginated roles")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for roles.", typeof(ApiResponse<PagedResult<RoleBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleBrowseDto>>>> BrowseRoles()
            => new ApiResponse<IEnumerable<RoleBrowseDto>>(System.Net.HttpStatusCode.OK, await _roleService.BrowseAsync());
    }
}
