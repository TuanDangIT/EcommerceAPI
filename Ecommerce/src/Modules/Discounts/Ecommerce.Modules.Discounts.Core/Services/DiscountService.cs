using Azure.Core;
using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.DAL.Mappings;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Ecommerce.Modules.Discounts.Core.Events;
using Ecommerce.Modules.Discounts.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Shared.Infrastructure.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sieve.Extensions.MethodInfoExtended;

namespace Ecommerce.Modules.Discounts.Core.Services
{
    internal class DiscountService : IDiscountService
    {
        private readonly IDiscountDbContext _dbContext;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly IMessageBroker _messageBroker;
        private readonly TimeProvider _timeProvider;

        public DiscountService(IDiscountDbContext dbContext, ISieveProcessor sieveProcessor, IMessageBroker messageBroker, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
        }

        public async Task<PagedResult<NominalDiscountBrowseDto>> BrowseNominalDiscountsAsync(SieveModel model)
        {
            var discounts = _dbContext.Discounts
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, discounts)
                .Where(d => d.Type == Entities.Enums.DiscountType.NominalDiscount)
                .Cast<NominalDiscount>()
                .Select(nd => nd.AsNominalDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(model, discounts, applyPagination: false, applySorting: false)
                .Where(d => d.Type == Entities.Enums.DiscountType.NominalDiscount)
                .CountAsync();
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<NominalDiscountBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task<PagedResult<PercentageDiscountBrowseDto>> BrowsePercentageDiscountsAsync(SieveModel model)
        {
            var discounts = _dbContext.Discounts
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, discounts)
                .Where(d => d.Type == Entities.Enums.DiscountType.PercentageDiscount)
                .Cast<PercentageDiscount>()
                .Select(pd => pd.AsPercentageDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(model, discounts, applyPagination: false, applySorting: false)
                .Where(d => d.Type == Entities.Enums.DiscountType.PercentageDiscount)
                .CountAsync();
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<PercentageDiscountBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task CreateAsync(NominalDiscountCreateDto dto)
        {
            var discount = await GetAsync(dto.Code);
            if(discount is not null)
            {
                throw new DiscountCodeAlreadyInUseException(dto.Code);
            }
            await _dbContext.Discounts.AddAsync
                (
                    dto.EndingDate is null 
                    ? new NominalDiscount(dto.Code, dto.NominalValue, _timeProvider.GetUtcNow().UtcDateTime) 
                    : new NominalDiscount(dto.Code, dto.NominalValue, (DateTime)dto.EndingDate, _timeProvider.GetUtcNow().UtcDateTime)
                );
            await _dbContext.SaveChangesAsync();
            await _messageBroker.PublishAsync(new DiscountCreated(dto.Code, DiscountType.NominalDiscount.ToString(), dto.NominalValue, dto.EndingDate));
        }

        public async Task CreateAsync(PercentageDiscountCreateDto dto)
        {
            var discount = await GetAsync(dto.Code);
            if (discount is not null)
            {
                throw new DiscountCodeAlreadyInUseException(dto.Code);
            }
            await _dbContext.Discounts.AddAsync
                (
                    dto.EndingDate is null 
                    ? new PercentageDiscount(dto.Code, dto.Percent, _timeProvider.GetUtcNow().UtcDateTime) 
                    : new PercentageDiscount(dto.Code, dto.Percent, (DateTime)dto.EndingDate,_timeProvider.GetUtcNow().UtcDateTime)
                );
            await _dbContext.SaveChangesAsync();
            await _messageBroker.PublishAsync(new DiscountCreated(dto.Code, DiscountType.PercentageDiscount.ToString(), dto.Percent, dto.EndingDate));
        }

        public async Task DeleteAsync(string code)
        {
            await _dbContext.Discounts.Where(d => d.Code == code)
                .ExecuteDeleteAsync();
            await _messageBroker.PublishAsync(new DiscountDeleted(code));
        }
        public async Task<Discount?> GetAsync(string code)
            => await _dbContext.Discounts.SingleOrDefaultAsync(d => d.Code == code);
    }
}
