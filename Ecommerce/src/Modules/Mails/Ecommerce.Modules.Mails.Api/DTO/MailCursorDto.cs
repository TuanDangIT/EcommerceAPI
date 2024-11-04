﻿using Ecommerce.Shared.Infrastructure.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.DTO
{
    internal class MailCursorDto
    {
        public int CursorId { get; set; }
        public DateTime CursorCreatedAt { get; set; }
        [ModelBinder(BinderType = typeof(DictionaryModelBinder))]
        public Dictionary<string, string>? Filters { get; set; }    
    }
}
