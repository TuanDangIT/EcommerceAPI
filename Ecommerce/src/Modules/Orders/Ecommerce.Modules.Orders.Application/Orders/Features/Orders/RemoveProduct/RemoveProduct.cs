using Ecommerce.Shared.Abstractions.MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.RemoveProduct
{
    public sealed record class RemoveProduct(int ProductId, int? Quantity) : ICommand
    {
        [SwaggerIgnore]
        public Guid OrderId { get; init; }
    }
}
