using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.BrowseManufacturers;
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
    internal sealed class BrowseManufacturersHandler : IQueryHandler<BrowseManufacturers, PagedResult<ManufacturerBrowseDto>>
    {
        private readonly InventoryDbContext _dbContext;
        private readonly ISieveProcessor _sieveProcessor;

        public BrowseManufacturersHandler(InventoryDbContext dbContext, ISieveProcessor sieveProcessor)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
        }
        public async Task<PagedResult<ManufacturerBrowseDto>> Handle(BrowseManufacturers request, CancellationToken cancellationToken)
        {
            var manufacturers = _dbContext.Manufacturers
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(request, manufacturers)
                .Select(c => c.AsBrowseDto())
                .AsNoTracking()
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(request, manufacturers, applyPagination: false, applySorting: false)
                .CountAsync();
            if (request.PageSize is null || request.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<ManufacturerBrowseDto>(dtos, totalCount, request.PageSize.Value, request.Page.Value);
            return pagedResult;
        }
    }
}
