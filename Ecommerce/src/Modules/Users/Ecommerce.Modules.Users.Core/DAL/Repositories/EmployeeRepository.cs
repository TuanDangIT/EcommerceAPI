﻿using Ecommerce.Modules.Users.Core.DAL.Mappings;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Entities.Enums;
using Ecommerce.Modules.Users.Core.Sieve;
using Ecommerce.Shared.Infrastructure.Pagination;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.EntityFrameworkCore;
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
        private readonly ISieveProcessor _sieveProcessor;

        public EmployeeRepository(UsersDbContext dbContext, IEnumerable<ISieveProcessor> sieveProcessors)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessors.First(s => s.GetType() == typeof(UsersModuleSieveProcessor));
        }
        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(employee, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<EmployeeBrowseDto>> GetAllAsync(SieveModel model, CancellationToken cancellationToken = default)
        {
            if (model.PageSize is null || model.Page is null)
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
            var pagedResult = new PagedResult<EmployeeBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
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
                .SingleOrDefaultAsync(u => u.Id == employeeId, cancellationToken);
        }
    }
}
