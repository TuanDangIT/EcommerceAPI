using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Abstractions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/" + InventoryModule.BasePath + "/[controller]")]
    internal abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        protected ActionResult<ApiResponse> OkOrNotFound<T>(object? model)
        {
            if (model is not null)
            {
                return Ok(new ApiResponse(HttpStatusCode.OK, "success", model));
            }
            string entityName = typeof(T).Name;
             return NotFound(new ExceptionResponse(HttpStatusCode.NotFound, "error", $"{entityName} was not found"));
        }
    }
}
