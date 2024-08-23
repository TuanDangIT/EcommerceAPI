using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Application.Features.Products.BrowseProducts;
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
    internal sealed class BrowseProductsHandler : IQueryHandler<BrowseProducts, PagedResult<ProductListingDto>>
    {
        private readonly InventoryDbContext _dbContext;
        private readonly ISieveProcessor _sieveProcessor;

        public BrowseProductsHandler(InventoryDbContext dbContext, ISieveProcessor sieveProcessor)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
        }
        public async Task<PagedResult<ProductListingDto>> Handle(BrowseProducts request, CancellationToken cancellationToken)
        {
            var products = _dbContext.Products
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.ProductParameters)
                .ThenInclude(pp => pp.Parameter)
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(request, products)
                .Select(p => p.AsListingDto())
                .AsNoTracking()
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(request, products, applyPagination: false, applyFiltering: false)
                .CountAsync();
            if(request.PageSize is null || request.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<ProductListingDto>(dtos, totalCount, request.PageSize.Value, request.Page.Value);
            return pagedResult;
        }
    }
}
