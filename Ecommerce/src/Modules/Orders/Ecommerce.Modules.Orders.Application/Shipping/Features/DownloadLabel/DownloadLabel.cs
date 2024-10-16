using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Shipping.Features.DownloadLabel
{
    public sealed record class DownloadLabel(int ShipmentId) : ICommand<(Stream FileStream, string MimeType, string FileName)>;
}
