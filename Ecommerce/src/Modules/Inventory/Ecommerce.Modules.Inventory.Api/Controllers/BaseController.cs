using Asp.Versioning;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Abstractions.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    [EnableRateLimiting("fixed-by-ip")]
    [ApiController]
    [Route("api/v{v:apiVersion}/" + InventoryModule.BasePath + "/[controller]")]
    internal abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        private const string NotFoundTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        protected ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse, TEntity>(TResponse? model)
        {
            if (model is not null)
            {
                return Ok(new ApiResponse<TResponse>(HttpStatusCode.OK, model));
            }
            string entityName = typeof(TEntity).Name;
            return NotFound(new ProblemDetails()
            {
                Type = NotFoundTypeUrl,
                Title = $"{entityName} was not found.",
                Status = (int)HttpStatusCode.NotFound
            });
        }
        protected ActionResult<ApiResponse<object>> Created(string id)
            => new ApiResponse<object>(HttpStatusCode.Created, new { Id = id});
    }
}
