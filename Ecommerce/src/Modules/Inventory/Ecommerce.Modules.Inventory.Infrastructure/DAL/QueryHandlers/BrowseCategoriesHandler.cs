using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Category.BrowseCategory;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.QueryHandlers
{
    internal sealed class BrowseCategoriesHandler : IQueryHandler<BrowseCategories, IEnumerable<CategoryDto>>
    {
        public Task<IEnumerable<CategoryDto>> Handle(BrowseCategories request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
