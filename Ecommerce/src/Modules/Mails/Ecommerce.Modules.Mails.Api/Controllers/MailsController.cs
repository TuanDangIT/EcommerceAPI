using Asp.Versioning;
using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Modules.Mails.Api.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.ModelBinders;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    [EnableRateLimiting("fixed-by-ip")]
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/" + MailsModule.BasePath + "/[controller]")]
    internal class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private const string _notFoundTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [SwaggerOperation("Sends email via company email")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpPost]
        public async Task<ActionResult> SendMail([FromForm]MailSendDefaultBodyDto dto)
        {
            await _mailService.SendAsync(dto);
            return NoContent();
        }
        [SwaggerOperation("Gets cursor paginated mails")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns cursor paginated result for mails.", typeof(ApiResponse<CursorPagedResult<MailBrowseDto, MailCursorDto>>))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<MailBrowseDto, MailCursorDto>>>> BrowseMails([FromQuery]MailCursorDto dto, bool? isNextPage, int pageSize, 
            CancellationToken cancellationToken = default)
            => Ok(new ApiResponse<CursorPagedResult<MailBrowseDto, MailCursorDto>>(HttpStatusCode.OK, 
                await _mailService.BrowseAsync(dto, isNextPage, pageSize, cancellationToken)));
        [SwaggerOperation("Get a specific mail")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a specific mail by id.", typeof(ApiResponse<MailDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Mail was not found")]
        [HttpGet("{mailId:int}")]
        public async Task<ActionResult<ApiResponse<MailDetailsDto>>> GetMail([FromRoute] int mailId)
            => OkOrNotFound(await _mailService.GetAsync(mailId), mailId, "Mail");
        private ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse>(TResponse? model, int mailId, string entityName)
        {
            if (model is not null)
            {
                return Ok(new ApiResponse<TResponse>(HttpStatusCode.OK, model));
            }
            return NotFound(new ProblemDetails()
            {
                Type = _notFoundTypeUrl,
                Title = $"{entityName}: {mailId} was not found.",
                Status = (int)HttpStatusCode.NotFound
            });
        }
    }
}
