using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Application.Features.Category.BrowseCategory;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.QueryHandlers
{
    internal sealed class BrowseCategoriesHandler : IQueryHandler<BrowseCategories, PagedResult<CategoryBrowseDto>>
    {
        private readonly InventoryDbContext _dbContext;
        private readonly ISieveProcessor _sieveProcessor;

        public BrowseCategoriesHandler(InventoryDbContext dbContext, ISieveProcessor sieveProcessor)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
        }
        public async Task<PagedResult<CategoryBrowseDto>> Handle(BrowseCategories request, CancellationToken cancellationToken)
        {
            var categories = _dbContext.Categories
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(request, categories)
                .Select(c => c.AsBrowseDto())
                .AsNoTracking()
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(request, categories, applyPagination: false, applySorting: false)
                .CountAsync();
            if (request.PageSize is null || request.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<CategoryBrowseDto>(dtos, totalCount, request.PageSize.Value, request.Page.Value);
            return pagedResult;

        }
    }
}
