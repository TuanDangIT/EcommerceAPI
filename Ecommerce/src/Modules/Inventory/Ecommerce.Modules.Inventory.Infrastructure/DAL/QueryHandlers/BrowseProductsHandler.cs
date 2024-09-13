﻿using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.BrowseProducts;
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
    internal sealed class BrowseProductsHandler : IQueryHandler<BrowseProducts, PagedResult<ProductBrowseDto>>
    {
        private readonly InventoryDbContext _dbContext;
        private readonly ISieveProcessor _sieveProcessor;

        public BrowseProductsHandler(InventoryDbContext dbContext, ISieveProcessor sieveProcessor)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
        }
        public async Task<PagedResult<ProductBrowseDto>> Handle(BrowseProducts request, CancellationToken cancellationToken)
        {
            var products = _dbContext.Products
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.ProductParameters)
                .ThenInclude(pp => pp.Parameter)
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(request, products)
                .Select(p => p.AsBrowseDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(request, products, applyPagination: false)
                .CountAsync();
            if(request.PageSize is null || request.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<ProductBrowseDto>(dtos, totalCount, request.PageSize.Value, request.Page.Value);
            return pagedResult;
        }
    }
}
