using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using Sieve.Models;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.BrowseProducts
{
    public sealed class BrowseProducts : SieveModel, IQuery<PagedResult<ProductListingDto>>;
}
