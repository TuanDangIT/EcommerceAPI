using Ecommerce.Shared.Abstractions.MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.SubmitComplaint
{
    public sealed record class SubmitComplaint(string Title, string Description) : ICommand
    {
        [SwaggerIgnore]
        public Guid OrderId { get; init; }
    }
}
