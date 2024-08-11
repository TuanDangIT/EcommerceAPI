using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Products.BrowseProducts;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.QueryHandlers
{
    internal sealed class BrowseProductsHandler : IQueryHandler<BrowseProducts, IEnumerable<ProductListingDto>>
    {
        public Task<IEnumerable<ProductListingDto>> Handle(BrowseProducts request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
