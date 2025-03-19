using Ecommerce.Shared.Abstractions.MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.RejectReturn
{
    public sealed record class RejectReturn(string RejectReason) : ICommand
    {
        [SwaggerIgnore] 
        public Guid ReturnId { get; init; }
    }
}
