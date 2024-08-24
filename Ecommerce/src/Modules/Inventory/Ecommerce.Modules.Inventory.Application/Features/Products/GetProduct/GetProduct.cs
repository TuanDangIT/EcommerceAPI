using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.GetProduct
{
    public sealed record class GetProduct(Guid ProductId) : IQuery<ProductDetailsDto?>;
}
