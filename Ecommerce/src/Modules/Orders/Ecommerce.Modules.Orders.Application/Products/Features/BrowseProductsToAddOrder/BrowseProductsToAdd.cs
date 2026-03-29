using Ecommerce.Modules.Orders.Application.Products.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Products.Features.BrowseProductsToAddToOrder
{
    public sealed record class BrowseProductsToAdd(string SearchTerm) : ICommand<IEnumerable<ProductToAddToOrderBrowseDto>>;
}
