using Asp.Versioning;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Api.Controllers
{
    [ApiController]
    [Route("api/v{v:apiVersion}/" + DiscountsModule.BasePath + "/[controller]")]
    internal abstract class BaseController : ControllerBase
    {
        private const string NotFoundTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";
        protected ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse>(TResponse? model, string entityName)
        {
            if (model is not null)
            {
                return Ok(new ApiResponse<TResponse>(HttpStatusCode.OK, model));
            }
            return NotFound(new ProblemDetails()
            {
                Type = NotFoundTypeUrl,
                Title = $"{entityName} was not found.",
                Status = (int)HttpStatusCode.NotFound
            });
        }
        protected ActionResult<ApiResponse<PagedResult<T>>> PagedResult<T>(PagedResult<T> model)
        {
            return Ok(new ApiResponse<PagedResult<T>>(HttpStatusCode.OK, model));
        }
        //private string FirstCharToUpper(string input) =>
        //    input switch
        //{
        //    null => throw new ArgumentNullException(nameof(input)),
        //    "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
        //    _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        //};
    }
}
