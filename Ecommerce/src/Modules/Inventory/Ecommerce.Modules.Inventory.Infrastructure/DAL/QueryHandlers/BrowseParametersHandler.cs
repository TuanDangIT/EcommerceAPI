using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.BrowseParameters;
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
    internal sealed class BrowseParametersHandler : IQueryHandler<BrowseParameters, PagedResult<ParameterBrowseDto>>
    {
        private readonly InventoryDbContext _dbContext;
        private readonly ISieveProcessor _sieveProcessor;

        public BrowseParametersHandler(InventoryDbContext dbContext, IEnumerable<ISieveProcessor> sieveProcessors)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessors.First(s => s.GetType() == typeof(InventoryModuleSieveProcessor));
        }
        public async Task<PagedResult<ParameterBrowseDto>> Handle(BrowseParameters request, CancellationToken cancellationToken)
        {
            if (request.PageSize is null || request.Page is null)
            {
                throw new PaginationException();
            }
            var parameters = _dbContext.Parameters
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(request, parameters)
                .Select(c => c.AsBrowseDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(request, parameters, applyPagination: false, applySorting: false)
                .CountAsync(cancellationToken);
            var pagedResult = new PagedResult<ParameterBrowseDto>(dtos, totalCount, request.PageSize.Value, request.Page.Value);
            return pagedResult;
        }
    }
}
