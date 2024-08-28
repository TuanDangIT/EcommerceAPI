using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.GetAllCategories
{
    public sealed record class GetAllCategories : IQuery<IEnumerable<CategoryOptionDto>>;
}
