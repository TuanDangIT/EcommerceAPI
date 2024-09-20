using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.GetReturn
{
    public sealed record class GetReturn(Guid ReturnId) : IQuery<ReturnDetailsDto>;
}
