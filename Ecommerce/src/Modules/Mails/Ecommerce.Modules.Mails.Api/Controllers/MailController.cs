using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Modules.Mails.Api.Services;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Controllers
{
    internal class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private const string NotFoundTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpPost]
        public async Task<ActionResult> SendMail([FromBody]MailSendDto dto)
        {
            await _mailService.SendAsync(dto);
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<MailBrowseDto, MailCursorDto>>>> BrowseMails([FromForm]MailCursorDto dto, bool? isNextPage, int pageSize)
        {
            var result = await _mailService.BrowseAsync(dto, isNextPage, pageSize);
            return Ok(new ApiResponse<CursorPagedResult<MailBrowseDto, MailCursorDto>>(HttpStatusCode.OK, result));
        }
        [HttpGet("{mailId:int}")]
        public async Task<ActionResult<ApiResponse<MailDetailsDto>>> GetMail([FromRoute]int mailId)
            => OkOrNotFound(await _mailService.GetAsync(mailId), mailId, "Mail");
        private ActionResult<ApiResponse<TResponse>> OkOrNotFound<TResponse>(TResponse? model, int mailId, string entityName)
        {
            if (model is not null)
            {
                return Ok(new ApiResponse<TResponse>(HttpStatusCode.OK, model));
            }
            return NotFound(new ProblemDetails()
            {
                Type = NotFoundTypeUrl,
                Title = $"{entityName}: {mailId} was not found.",
                Status = (int)HttpStatusCode.NotFound
            });
        }
    }
}
