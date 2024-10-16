using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Invoices.Features.DownloadInvoice
{
    public sealed record class DownloadInvoice(int InvoiceId) : ICommand<(Stream FileStream, string MimeType, string FileName)>;
}
