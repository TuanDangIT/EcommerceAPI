using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.BrowseCategories
{
    public sealed class BrowseCategories : SieveModel, IQuery<PagedResult<CategoryBrowseDto>>;
}
