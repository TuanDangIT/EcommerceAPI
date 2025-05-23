﻿using Asp.Versioning;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Api.Controllers
{
    [EnableRateLimiting("fixed-by-ip")]
    [ApiController]
    [Route("api/v{v:apiVersion}/" + CartsModule.BasePath + "/[controller]")]
    internal abstract class BaseController : ControllerBase
    {
        private const string _notFoundTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";
        protected ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse>(TResponse? model, string entityName, string id)
        {
            if (model is not null)
            {
                return Ok(new ApiResponse<TResponse>(HttpStatusCode.OK, model));
            }
            return NotFound(new ProblemDetails()
            {
                Type = _notFoundTypeUrl,
                Title = $"An exception occurred.",
                Status = (int)HttpStatusCode.NotFound,
                Detail = $"{entityName}: {id} was not found."
            });
        }
    }
}
