using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.DownloadInvoice
{
    public sealed record class DownloadInvoice(Guid OrderId) : IQuery<(Stream FileStream, string MimeType, string FileName)>;
}
