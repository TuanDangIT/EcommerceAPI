using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.DAL.Mappings;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Ecommerce.Modules.Discounts.Core.Events;
using Ecommerce.Modules.Discounts.Core.Exceptions;
using Ecommerce.Modules.Discounts.Core.Services.Externals;
using Ecommerce.Modules.Discounts.Core.Sieve;
using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Services
{
    internal class CouponService : ICouponService
    {
        private readonly IDiscountDbContext _dbContext;
        private readonly IStripeService _stripeService;
        private readonly IMessageBroker _messageBroker;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly TimeProvider _timeProvider;

        public CouponService(IDiscountDbContext dbContext, IStripeService stripeService, IMessageBroker messageBroker, IEnumerable<ISieveProcessor> sieveProcessors, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _stripeService = stripeService;
            _messageBroker = messageBroker;
            _sieveProcessor = sieveProcessors.First(s => s.GetType() == typeof(DiscountsModuleSieveProcessor));
            _timeProvider = timeProvider;
        }
        public async Task<PagedResult<NominalCouponBrowseDto>> BrowseNominalCouponsAsync(SieveModel model)
        {
            var coupons = _dbContext.Coupons
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, coupons)
                .Where(c => c.Type == CouponType.NominalCoupon)
                .Cast<NominalCoupon>()
                .Select(nd => nd.AsNominalBrowseDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(model, coupons, applyPagination: false, applySorting: false)
                .Where(c => c.Type == CouponType.NominalCoupon)
                .CountAsync();
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<NominalCouponBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task<PagedResult<PercentageCouponBrowseDto>> BrowsePercentageCouponsAsync(SieveModel model)
        {
            var coupons = _dbContext.Coupons
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, coupons)
                .Where(c => c.Type == CouponType.PercentageCoupon)
                .Cast<PercentageCoupon>()
                .Select(nd => nd.AsPercentageBrowseDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(model, coupons, applyPagination: false, applySorting: false)
                .Where(c => c.Type == CouponType.NominalCoupon)
                .CountAsync();
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<PercentageCouponBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task CreateAsync(NominalCouponCreateDto dto)
        {
            var stripeCouponId = await _stripeService.CreateCouponAsync(dto);
            await _dbContext.Coupons.AddAsync(new NominalCoupon(dto.Name, dto.NominalValue, _timeProvider.GetUtcNow().UtcDateTime, stripeCouponId));
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(PercentageCouponCreateDto dto)
        {
            var stripeCouponId = await _stripeService.CreateCouponAsync(dto);
            await _dbContext.Coupons.AddAsync(new PercentageCoupon(dto.Name, dto.Percent, _timeProvider.GetUtcNow().UtcDateTime, stripeCouponId));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string stripeCouponId)
        {
            var coupon = await _dbContext.Coupons
                .Include(c => c.Discounts)
                .SingleOrDefaultAsync(c => c.StripeCouponId == stripeCouponId);
            if(coupon is null)
            {
                throw new CouponNotFoundException(stripeCouponId);
            }
            if (coupon.Discounts.Any())
            {
                foreach(var discount in coupon.Discounts)
                {
                    await _messageBroker.PublishAsync(new DiscountDeactivated(discount.Code));
                }
            }
            await _stripeService.DeleteCouponAsync(stripeCouponId);
            _dbContext.Coupons.Remove(coupon);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateNameAsync(string stripeCouponId, CouponUpdateNameDto dto)
        {
            var coupon = await _dbContext.Coupons.SingleOrDefaultAsync(c => c.StripeCouponId == stripeCouponId);
            if(coupon is null)
            {
                throw new CouponNotFoundException(stripeCouponId);
            }
            coupon.ChangeName(dto.Name, _timeProvider.GetUtcNow().UtcDateTime);
            await _stripeService.UpdateCouponName(stripeCouponId, dto.Name);
            await _dbContext.SaveChangesAsync();
        }
    }
}
