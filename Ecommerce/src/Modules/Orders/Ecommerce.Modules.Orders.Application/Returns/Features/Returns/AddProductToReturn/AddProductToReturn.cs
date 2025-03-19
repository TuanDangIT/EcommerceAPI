using Ecommerce.Shared.Abstractions.MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.AddProductToReturn
{
    public sealed record class AddProductToReturn(string SKU, int Quantity) : ICommand
    {
        [SwaggerIgnore]
        public Guid ReturnId { get; init; }
    }
}
