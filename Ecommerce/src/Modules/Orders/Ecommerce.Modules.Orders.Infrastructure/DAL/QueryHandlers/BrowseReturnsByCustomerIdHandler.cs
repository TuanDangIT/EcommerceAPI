using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Modules.Orders.Application.Returns.Features.Returns.BrowseReturnsByCustomerId;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Contexts;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class BrowseReturnsByCustomerIdHandler : IQueryHandler<BrowseReturnsByCustomerId, PagedResult<ReturnBrowseDto>>
    {
        private readonly OrdersDbContext _dbContext;
        private readonly IOptions<SieveOptions> _sieveOptions;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly IContextService _contextService;

        public BrowseReturnsByCustomerIdHandler(OrdersDbContext dbContext, IOptions<SieveOptions> sieveOptions,
            [FromKeyedServices("orders-sieve-processor")] ISieveProcessor sieveProcessor, IContextService contextService)
        {
            _dbContext = dbContext;
            _sieveOptions = sieveOptions;
            _sieveProcessor = sieveProcessor;
            _contextService = contextService;
        }

        public async Task<PagedResult<ReturnBrowseDto>> Handle(BrowseReturnsByCustomerId request, CancellationToken cancellationToken)
        {
            if (request.CustomerId != _contextService.Identity!.Id)
            {
                throw new CustomerNotAuthorizedException(_contextService.Identity!.Id);
            }
            if (request.Page is null)
            {
                throw new PaginationException();
            }
            var returns = _dbContext.Returns
                .Include(r => r.Order)
                .ThenInclude(o => o.Customer)
                .Where(r => r.Order.Customer.UserId == request.CustomerId)
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(request, returns)
                .Select(r => r.AsBrowseDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(request, returns, applyPagination: false)
                .CountAsync(cancellationToken);
            int pageSize = _sieveOptions.Value.DefaultPageSize;
            if (request.PageSize is not null)
            {
                pageSize = request.PageSize.Value;
            }
            var pagedResult = new PagedResult<ReturnBrowseDto>(dtos, totalCount, pageSize, request.Page.Value);
            return pagedResult;
        }
    }
}
