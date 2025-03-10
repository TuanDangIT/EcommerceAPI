using Ecommerce.Modules.Users.Core.DAL.Mappings;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Entities.Enums;
using Ecommerce.Modules.Users.Core.Sieve;
using Ecommerce.Shared.Infrastructure.Pagination;
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

namespace Ecommerce.Modules.Users.Core.DAL.Repositories
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        private readonly UsersDbContext _dbContext;
        private readonly IOptions<SieveOptions> _sieveOptions;
        private readonly ISieveProcessor _sieveProcessor;

        public EmployeeRepository(UsersDbContext dbContext, [FromKeyedServices("users-sieve-processor")] ISieveProcessor sieveProcessor, IOptions<SieveOptions> sieveOptions)
        {
            _dbContext = dbContext;
            _sieveOptions = sieveOptions;
            _sieveProcessor = sieveProcessor;
        }

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(employee, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<EmployeeBrowseDto>> GetAllAsync(SieveModel model, CancellationToken cancellationToken = default)
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
                .Where(u => u.Type == UserType.Employee)
                .Cast<Employee>()
                .Select(c => c.AsBrowseDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(model, coupons, applyPagination: false, applySorting: false)
                .Where(u => u.Type == UserType.Employee)
                .CountAsync(cancellationToken);
            int pageSize = _sieveOptions.Value.DefaultPageSize;
            if (model.PageSize is not null)
            {
                pageSize = model.PageSize.Value;
            }
            var pagedResult = new PagedResult<EmployeeBrowseDto>(dtos, totalCount, pageSize, model.Page.Value);
            return pagedResult;
        }

        public async Task<Employee?> GetAsync(Guid employeeId, bool asNoTracking, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Users
                .Where(u => u.Type == UserType.Employee)
                .Cast<Employee>()
                .AsQueryable();
            if (asNoTracking)
            {
                query.AsNoTracking();
            }
            return await query
                .FirstOrDefaultAsync(u => u.Id == employeeId, cancellationToken);
        }
    }
}
