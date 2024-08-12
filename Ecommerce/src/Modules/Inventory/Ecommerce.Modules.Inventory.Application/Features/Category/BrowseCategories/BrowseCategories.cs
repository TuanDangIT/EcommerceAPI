using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Category.BrowseCategory
{
    public sealed record class BrowseCategories : IQuery<IEnumerable<CategoryDto>>;
}
