using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Category.GetAllCategories;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.QueryHandlers
{
    internal sealed class GetAllCategoriesHandler : IQueryHandler<GetAllCategories, IEnumerable<CategoryOptionDto>>
    {
        private readonly InventoryDbContext _dbContext;

        public GetAllCategoriesHandler(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<CategoryOptionDto>> Handle(GetAllCategories request, CancellationToken cancellationToken)
            => await _dbContext.Categories.Select(c => c.AsOptionDto()).ToListAsync();
    }
}
