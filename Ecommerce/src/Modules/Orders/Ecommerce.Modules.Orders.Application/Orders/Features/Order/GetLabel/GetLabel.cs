using Ecommerce.Modules.Orders.Application.Delivery;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.GetLabel
{
    public sealed record class GetLabel(Guid OrderId) : ICommand<(Stream FileStream, string MimeType, string FileName)>;
}
