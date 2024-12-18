using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.BrowseManufacturers;
using Ecommerce.Modules.Inventory.Application.Shared.Sieve;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
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

        public BrowseManufacturersHandler(InventoryDbContext dbContext, IEnumerable<ISieveProcessor> sieveProcessors)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessors.First(s => s.GetType() == typeof(InventoryModuleSieveProcessor));
        }
        public async Task<PagedResult<ManufacturerBrowseDto>> Handle(BrowseManufacturers request, CancellationToken cancellationToken)
        {
            if (request.PageSize is null || request.Page is null)
            {
                throw new PaginationException();
            }
            var manufacturers = _dbContext.Manufacturers
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(request, manufacturers)
                .Select(c => c.AsBrowseDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(request, manufacturers, applyPagination: false, applySorting: false)
                .CountAsync(cancellationToken);
            var pagedResult = new PagedResult<ManufacturerBrowseDto>(dtos, totalCount, request.PageSize.Value, request.Page.Value);
            return pagedResult;
        }
    }
}
