using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Services;
using Ecommerce.Shared.Abstractions.Auth;
using Ecommerce.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Api.Controllers
{
    internal class UserController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly IContextService _contextService;

        public UserController(IIdentityService identityService, IContextService contextService)
        {
            _identityService = identityService;
            _contextService = contextService;
        }
        [HttpPost("sign-in")]
        public async Task<ActionResult<JsonWebToken>> SignIn([FromBody]SignInDto dto)
        {
            return Ok(await _identityService.SignInAsync(dto));
        }
        [HttpPost("sign-up")]
        public async Task<ActionResult> SignUp([FromBody]SignUpDto dto)
        {
            await _identityService.SignUpAsync(dto);
            return NoContent();
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto?>> GetAsync()
            => OkOrNotFound(await _identityService.GetAsync(_contextService.Identity!.Id));
    }
}
