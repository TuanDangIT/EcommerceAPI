﻿using Ecommerce.Modules.Orders.Application.Invoices.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.ModelBinders;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Invoices.Features.BrowseInvoices
{
    public sealed record class BrowseInvoices(InvoiceCursorDto? CursorDto, bool? IsNextPage, int PageSize) : IQuery<CursorPagedResult<InvoiceBrowseDto, InvoiceCursorDto>>
    {
        [ModelBinder(BinderType = typeof(DictionaryModelBinder))]
        public Dictionary<string, string>? Filters { get; set; }
    }
}
