using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Api.Controllers
{
    [ApiController]
    [Route("api/" + UsersModule.BasePath + "/[controller]")]
    internal abstract class BaseController : ControllerBase
    {
        private const string NotFoundTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";
        protected ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse, TEntity>(TResponse? model)
        {
            if (model is not null)
            {
                return Ok(new ApiResponse<TResponse>(HttpStatusCode.OK, "success", model));
            }
            string entityName = typeof(TEntity).Name;
            return NotFound(new ProblemDetails()
            {
                Type = NotFoundTypeUrl,
                Title = $"{entityName} was not found.",
                Status = (int)HttpStatusCode.NotFound
            });
        }
    }
}
