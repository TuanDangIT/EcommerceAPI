using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.WriteAdditionalInformation
{
    public sealed record class WriteAdditionalInformation(Guid OrderId, string AdditionalInformation) : ICommand;
}
