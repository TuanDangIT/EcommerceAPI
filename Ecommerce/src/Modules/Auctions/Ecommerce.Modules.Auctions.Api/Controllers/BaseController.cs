using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Api.Controllers
{
    [ApiController]
    [Route("api/" + AuctionsModule.BasePath + "/[controller]")]
    internal abstract class BaseController : ControllerBase
    {
        protected ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse, TEntity>(TResponse? model)
        {
            if (model is not null)
            {
                return Ok(new ApiResponse<TResponse>(HttpStatusCode.OK, "success", model));
            }
            string entityName = typeof(TEntity).Name;
            return NotFound(new ExceptionResponse(HttpStatusCode.NotFound, "error", $"{entityName} was not found"));
        }
    }
}
