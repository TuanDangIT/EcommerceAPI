﻿using Ecommerce.Modules.Users.Core.DAL.Mappings;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Entities.Enums;
using Ecommerce.Shared.Infrastructure.Pagination;
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
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly UsersDbContext _dbContext;
        private readonly ISieveProcessor _sieveProcessor;

        public CustomerRepository(UsersDbContext dbContext, ISieveProcessor sieveProcessor)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
        }
        public async Task<PagedResult<CustomerBrowseDto>> GetAllAsync(SieveModel model)
        {
            var coupons = _dbContext.Users
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, coupons)
                .Where(u => u.Type == UserType.Customer)
                .Cast<Customer>()
                .Select(c => c.AsBrowseDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(model, coupons, applyPagination: false, applySorting: false)
                .Where(u => u.Type == UserType.Customer)
                .CountAsync();
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<CustomerBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task<Customer?> GetAsync(Guid customerId, bool asNoTracking)
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
                .SingleOrDefaultAsync(u => u.Id == customerId);
        }
    }
}
