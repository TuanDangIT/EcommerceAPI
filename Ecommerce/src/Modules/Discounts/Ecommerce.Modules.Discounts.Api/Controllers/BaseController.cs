﻿using Asp.Versioning;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Api.Controllers
{
    [EnableRateLimiting("fixed-by-ip")]
    [ApiController]
    [Route("api/v{v:apiVersion}/" + DiscountsModule.BasePath + "/[controller]")]
    internal abstract class BaseController : ControllerBase
    {
        private const string _notFoundTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";
        protected ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse>(TResponse? model, string entityName, string entityId)
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
                Detail = $"{entityName}: {entityId} was not found."
            });
        }
        protected ActionResult<ApiResponse<PagedResult<T>>> PagedResult<T>(PagedResult<T> model)
        {
            return Ok(new ApiResponse<PagedResult<T>>(HttpStatusCode.OK, model));
        }
        protected ActionResult<ApiResponse<object>> Created(int id)
            => Created(default(string), new ApiResponse<object>(HttpStatusCode.Created, new { Id = id }));
        //private string FirstCharToUpper(string input) =>
        //    input switch
        //{
        //    null => throw new ArgumentNullException(nameof(input)),
        //    "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
        //    _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        //};
    }
}
