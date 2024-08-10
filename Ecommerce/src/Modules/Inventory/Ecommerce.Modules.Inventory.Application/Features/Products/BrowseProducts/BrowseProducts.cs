using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Shared.Abstractions.MediatR;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.BrowseProducts
{
    public sealed record class BrowseProducts : IQuery<IEnumerable<ProductDto>>;
}
