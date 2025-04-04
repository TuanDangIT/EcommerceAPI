using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
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
        Task<FileStreamResult> DownloadAttachmentFileAsync(int mailId, string fileId);
        Task<CursorPagedResult<MailBrowseDto, MailCursorDto>> BrowseCursorAsync(MailCursorDto cursorDto, CancellationToken cancellationToken = default);
        Task<PagedResult<MailBrowseDto>> BrowseOffsetAsync(SieveModel model, CancellationToken cancellationToken = default);
        Task<MailDetailsDto> GetAsync(int mailId, CancellationToken cancellation = default);
    }
}
