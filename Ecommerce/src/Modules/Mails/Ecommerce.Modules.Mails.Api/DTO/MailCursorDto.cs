using Ecommerce.Modules.Mails.Api.DataAnnotations;
using Ecommerce.Shared.Infrastructure.ModelBinders;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.DTO
{
    internal class MailCursorDto : CursorDto<int>
    {
        public bool? IsNextPage { get; set; }
        public int PageSize { get; set; }
        public DateTime CursorCreatedAt { get; set; }
        [MailCursor]
        [ModelBinder(BinderType = typeof(DictionaryModelBinder))]
        public Dictionary<string, string>? Filters { get; set; }    
    }
}
