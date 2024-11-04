using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Services
{
    internal interface IMailService
    {
        Task SendAsync(MailSendDto dto);
        Task SendAsync(MailSendDefaultBodyDto dto);
        Task<CursorPagedResult<MailBrowseDto, MailCursorDto>> BrowseAsync(MailCursorDto cursorDto, bool? IsNextPage, int PageSize);
        Task<MailDetailsDto> GetAsync(int mailId);
    }
}
