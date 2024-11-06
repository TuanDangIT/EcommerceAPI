using Azure.Core;
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
        private readonly IStripeService _stripeService;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly IMessageBroker _messageBroker;
        private readonly TimeProvider _timeProvider;

        public DiscountService(IDiscountDbContext dbContext, IStripeService stripeService, IEnumerable<ISieveProcessor> sieveProcessors, IMessageBroker messageBroker, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _stripeService = stripeService;
            _sieveProcessor = sieveProcessors.First(s => s.GetType() == typeof(DiscountsModuleSieveProcessor));
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
        }


        public async Task<PagedResult<DiscountBrowseDto>> BrowseDiscountsAsync(string stripeCouponId, SieveModel model)
        {
            var coupon = await _dbContext.Coupons.SingleOrDefaultAsync(c => c.StripeCouponId == stripeCouponId);
            if (coupon is null)
            {
                throw new CouponNotFoundException(stripeCouponId);
            }
            var discounts = _dbContext.Discounts
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, discounts)
                .Where(d => d.CouponId == coupon.Id)
                .Select(d => d.AsBrowseDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(model, discounts, applyPagination: false, applySorting: false)
                .Where(d => d.CouponId == coupon.Id)
                .CountAsync();
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<DiscountBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task CreateAsync(string stripeCouponId, DiscountCreateDto dto)
        {
            var coupon = await _dbContext.Coupons.SingleOrDefaultAsync(c => c.StripeCouponId == stripeCouponId);
            if(coupon is null)
            {
                throw new CouponNotFoundException(stripeCouponId);
            }
            var discount = await _dbContext.Discounts.SingleOrDefaultAsync(d => d.Code == dto.Code);
            if (discount is not null)
            {
                throw new DiscountCodeAlreadyInUseException(dto.Code);
            }
            var stripePromotionCodeId = await _stripeService.CreateDiscountAsync(stripeCouponId, dto);
            var now = _timeProvider.GetUtcNow().UtcDateTime;
            coupon.AddDiscount(new Discount(dto.Code, stripePromotionCodeId, dto.EndingDate, now), now);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ActivateAsync(string code)
        {
            var discount = await GetDiscountOrThrowIfNull(code);
            if(discount.IsActive is true)
            {
                throw new DiscountAlreadyActivated(code);
            }
            await _stripeService.ActivateDiscountAsync(discount.StripePromotionCodeId);
            discount.Activate(_timeProvider.GetUtcNow().UtcDateTime);
            await _dbContext.SaveChangesAsync();
            var coupon = discount.Coupon;
            switch (coupon)
            {
                case NominalCoupon nominalCoupon:
                    await _messageBroker.PublishAsync(
                        new DiscountActivated(discount.Code, nominalCoupon.Type.ToString(), discount.StripePromotionCodeId, nominalCoupon.NominalValue, discount.ExpiresAt));
                    break;
                case PercentageCoupon percentageCoupon:
                    await _messageBroker.PublishAsync(
                        new DiscountActivated(discount.Code, percentageCoupon.Type.ToString(), discount.StripePromotionCodeId, percentageCoupon.Percent, discount.ExpiresAt));
                    break;
            }
        }

        public async Task DeactivateAsync(string code)
        {
            var discount = await GetDiscountOrThrowIfNull(code);
            if(discount.IsActive is false)
            {
                throw new DiscountAlreadyDeactivated(code); 
            }
            await _stripeService.DeactivateDiscountAsync(discount.StripePromotionCodeId);
            discount.Deactive(_timeProvider.GetUtcNow().UtcDateTime);
            await _dbContext.SaveChangesAsync();
            await _messageBroker.PublishAsync(new DiscountDeactivated(discount.Code));
        }
        private async Task<Discount> GetDiscountOrThrowIfNull(string code)
        {
            var discount = await _dbContext.Discounts
                .Include(d => d.Coupon)
                .SingleOrDefaultAsync(d => d.Code == code);
            if (discount is null)
            {
                throw new DiscountNotFoundException(code);
            }
            return discount;
        }
        //public async Task ActivateAsync(string stripePromotionCodeId)
        //{
        //    var discount = await GetDiscountOrThrowIfNull(stripePromotionCodeId);
        //    await _stripeService.ActivateDiscountAsync(stripePromotionCodeId);
        //    discount.Activate(_timeProvider.GetUtcNow().UtcDateTime);
        //    await _dbContext.SaveChangesAsync();
        //    var coupon = discount.Coupon;
        //    switch (coupon)
        //    {
        //        case NominalCoupon nominalCoupon:
        //            await _messageBroker.PublishAsync(
        //                new DiscountActivated(discount.Code, nominalCoupon.Type.ToString(), discount.StripePromotionCodeId, nominalCoupon.NominalValue, discount.ExpiresAt));
        //            break;
        //        case PercentageCoupon percentageCoupon:
        //            await _messageBroker.PublishAsync(
        //                new DiscountActivated(discount.Code, percentageCoupon.Type.ToString(), discount.StripePromotionCodeId, percentageCoupon.Percent, discount.ExpiresAt));
        //            break;
        //    }
        //}

        //public async Task DeactivateAsync(string stripePromotionCodeId)
        //{
        //    var discount = await GetDiscountOrThrowIfNull(stripePromotionCodeId);
        //    await _stripeService.DeactivateDiscountAsync(stripePromotionCodeId);
        //    discount.Deactive(_timeProvider.GetUtcNow().UtcDateTime);
        //    await _dbContext.SaveChangesAsync();
        //    await _messageBroker.PublishAsync(new DiscountDeactivated(discount.Code));
        //}
        //private async Task<Discount> GetDiscountOrThrowIfNull(string stripePromotionCodeId)
        //{
        //    var discount = await _dbContext.Discounts
        //        .Include(d => d.Coupon)
        //        .SingleOrDefaultAsync(d => d.StripePromotionCodeId == stripePromotionCodeId);
        //    if (discount is null)
        //    {
        //        throw new DiscountNotFoundException(stripePromotionCodeId);
        //    }
        //    return discount;
        //}




        //public async Task<PagedResult<NominalDiscountBrowseDto>> BrowseNominalDiscountsAsync(SieveModel model)
        //{
        //    var discounts = _dbContext.Discounts
        //        .AsNoTracking()
        //        .AsQueryable();
        //    var dtos = await _sieveProcessor
        //        .Apply(model, discounts)
        //        .Where(d => d.Type == Entities.Enums.CouponType.NominalDiscount)
        //        .Cast<NominalCoupon>()
        //        .Select(nd => nd.AsNominalDto())
        //        .ToListAsync();
        //    var totalCount = await _sieveProcessor
        //        .Apply(model, discounts, applyPagination: false, applySorting: false)
        //        .Where(d => d.Type == Entities.Enums.CouponType.NominalDiscount)
        //        .CountAsync();
        //    if (model.PageSize is null || model.Page is null)
        //    {
        //        throw new PaginationException();
        //    }
        //    var pagedResult = new PagedResult<NominalDiscountBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
        //    return pagedResult;
        //}

        //public async Task<PagedResult<PercentageDiscountBrowseDto>> BrowsePercentageDiscountsAsync(SieveModel model)
        //{
        //    var discounts = _dbContext.Discounts
        //        .AsNoTracking()
        //        .AsQueryable();
        //    var dtos = await _sieveProcessor
        //        .Apply(model, discounts)
        //        .Where(d => d.Type == Entities.Enums.CouponType.PercentageDiscount)
        //        .Cast<PercentageCoupon>()
        //        .Select(pd => pd.AsPercentageDto())
        //        .ToListAsync();
        //    var totalCount = await _sieveProcessor
        //        .Apply(model, discounts, applyPagination: false, applySorting: false)
        //        .Where(d => d.Type == Entities.Enums.CouponType.PercentageDiscount)
        //        .CountAsync();
        //    if (model.PageSize is null || model.Page is null)
        //    {
        //        throw new PaginationException();
        //    }
        //    var pagedResult = new PagedResult<PercentageDiscountBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
        //    return pagedResult;
        //}

        //public async Task CreateAsync(NominalDiscountCreateDto dto)
        //{
        //    var discount = await GetAsync(dto.Code);
        //    if(discount is not null)
        //    {
        //        throw new DiscountCodeAlreadyInUseException(dto.Code);
        //    }
        //    await _dbContext.Discounts.AddAsync
        //        (
        //            dto.EndingDate is null 
        //            ? new NominalCoupon(dto.Code, dto.NominalValue, _timeProvider.GetUtcNow().UtcDateTime) 
        //            : new NominalCoupon(dto.Code, dto.NominalValue, (DateTime)dto.EndingDate, _timeProvider.GetUtcNow().UtcDateTime)
        //        );
        //    await _dbContext.SaveChangesAsync();
        //    await _messageBroker.PublishAsync(new DiscountCreated(dto.Code, CouponType.NominalDiscount.ToString(), dto.NominalValue, dto.EndingDate));
        //}

        //public async Task CreateAsync(PercentageDiscountCreateDto dto)
        //{
        //    var discount = await GetAsync(dto.Code);
        //    if (discount is not null)
        //    {
        //        throw new DiscountCodeAlreadyInUseException(dto.Code);
        //    }
        //    await _dbContext.Discounts.AddAsync
        //        (
        //            dto.EndingDate is null 
        //            ? new PercentageCoupon(dto.Code, dto.Percent, _timeProvider.GetUtcNow().UtcDateTime) 
        //            : new PercentageCoupon(dto.Code, dto.Percent, (DateTime)dto.EndingDate,_timeProvider.GetUtcNow().UtcDateTime)
        //        );
        //    await _dbContext.SaveChangesAsync();
        //    await _messageBroker.PublishAsync(new DiscountCreated(dto.Code, CouponType.PercentageDiscount.ToString(), dto.Percent, dto.EndingDate));
        //}

        //public async Task DeleteAsync(string code)
        //{
        //    await _dbContext.Discounts.Where(d => d.Code == code)
        //        .ExecuteDeleteAsync();
        //    await _messageBroker.PublishAsync(new DiscountDeleted(code));
        //}
        //public async Task<Discount?> GetAsync(string code)
        //    => await _dbContext.Discounts.SingleOrDefaultAsync(d => d.Code == code);
    }
}
