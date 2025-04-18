﻿using Asp.Versioning;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Abstractions.Auth;
using Ecommerce.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Api.Controllers
{
    [ApiVersion(1)]
    internal class UsersController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly IContextService _contextService;

        public UsersController(IIdentityService identityService, IContextService contextService)
        {
            _identityService = identityService;
            _contextService = contextService;
        }
        [HttpPost("sign-in")]
        public async Task<ActionResult<ApiResponse<JsonWebToken>>> SignIn([FromBody] SignInDto dto)
            => Ok(new ApiResponse<JsonWebToken>(HttpStatusCode.OK, await _identityService.SignInAsync(dto)));
        [HttpPost("sign-up")]
        public async Task<ActionResult> SignUp([FromBody] SignUpDto dto)
        {
            await _identityService.SignUpAsync(dto);
            return Created();
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserDto?>>> GetAsync()
            => OkOrNotFound<UserDto?>(await _identityService.GetAsync(_contextService.Identity!.Id), "User");
    }
}
