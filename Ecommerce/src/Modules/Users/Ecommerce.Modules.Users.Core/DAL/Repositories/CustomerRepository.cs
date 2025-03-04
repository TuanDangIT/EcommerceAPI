using Ecommerce.Modules.Users.Core.DAL.Mappings;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Entities.Enums;
using Ecommerce.Modules.Users.Core.Sieve;
using Ecommerce.Shared.Infrastructure.Pagination;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DAL.Repositories
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly UsersDbContext _dbContext;
        private readonly IOptions<SieveOptions> _sieveOptions;
        private readonly ISieveProcessor _sieveProcessor;

        public CustomerRepository(UsersDbContext dbContext, IEnumerable<ISieveProcessor> sieveProcessors, IOptions<SieveOptions> sieveOptions)
        {
            _dbContext = dbContext;
            _sieveOptions = sieveOptions;
            _sieveProcessor = sieveProcessors.First(s => s.GetType() == typeof(UsersModuleSieveProcessor));
        }
        public async Task<PagedResult<CustomerBrowseDto>> GetAllAsync(SieveModel model, CancellationToken cancellationToken = default)
        {
            if (model.Page is null)
            {
                throw new PaginationException();
            }
            var coupons = _dbContext.Users
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, coupons)
                .Where(u => u.Type == UserType.Customer)
                .Cast<Customer>()
                .Select(c => c.AsBrowseDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(model, coupons, applyPagination: false, applySorting: false)
                .Where(u => u.Type == UserType.Customer)
                .CountAsync(cancellationToken);
            int pageSize = _sieveOptions.Value.DefaultPageSize;
            if (model.PageSize is not null)
            {
                pageSize = model.PageSize.Value;
            }
            var pagedResult = new PagedResult<CustomerBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task<Customer?> GetAsync(Guid customerId, bool asNoTracking, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Users
                .Where(u => u.Type == UserType.Customer)
                .Cast<Customer>()
                .AsQueryable();
            if (asNoTracking)
            {
                query.AsNoTracking();
            }
            return await query
                .SingleOrDefaultAsync(u => u.Id == customerId, cancellationToken);
        }
    }
}
