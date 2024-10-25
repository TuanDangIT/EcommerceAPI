using Asp.Versioning;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Api.Controllers
{
    [ApiVersion(1)]
    internal class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleBrowseDto>>>> BrowseRoles()
            => new ApiResponse<IEnumerable<RoleBrowseDto>>(System.Net.HttpStatusCode.OK, await _roleService.BrowseAsync());
    }
}
