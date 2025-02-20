using Asp.Versioning;
using Ecommerce.Modules.Users.Core.DAL.Repositories;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Abstractions.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Api.Controllers
{
    [Authorize]
    [ApiVersion(1)]
    internal class TokenController : BaseController
    {
        private readonly IIdentityService _identityService;

        public TokenController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<JsonWebToken>>> RefreshToken(TokenDto dto)
        {
            var jwt = await _identityService.RefreshTokenAsync(dto);
            return Ok(new ApiResponse<JsonWebToken>(HttpStatusCode.OK, jwt));
        }
    }
}
