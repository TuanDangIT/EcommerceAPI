using Asp.Versioning;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    [EnableRateLimiting("fixed-by-ip")]
    [ApiController]
    [Route("api/v{v:apiVersion}/" + OrdersModule.BasePath + "/[controller]")]
    internal abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        private const string _notFoundTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        protected ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse, TEntity>(TResponse? model, string entityId)
        {
            if (model is not null)
            {
                return Ok(new ApiResponse<TResponse>(HttpStatusCode.OK, model));
            }
            string entityName = typeof(TEntity).Name;
            return NotFound(new ProblemDetails()
            {
                Type = _notFoundTypeUrl,
                Title = "An exception occurred.",
                Status = (int)HttpStatusCode.NotFound,
                Detail = $"{entityName}: {entityId} was not found."
            });
        }
        protected ActionResult<ApiResponse<CursorPagedResult<TData, TCursor>>> CursorPagedResult<TData, TCursor>(CursorPagedResult<TData, TCursor> model)
        {
            return Ok(new ApiResponse<CursorPagedResult<TData, TCursor>>(HttpStatusCode.OK, model));
        }
    }
}
