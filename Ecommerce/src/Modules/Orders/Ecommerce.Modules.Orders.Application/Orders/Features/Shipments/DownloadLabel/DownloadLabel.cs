using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.DownloadLabel
{
    public sealed record class DownloadLabel(Guid OrderId, int ShipmentId) : ICommand<(Stream FileStream, string MimeType, string FileName)>;
}
