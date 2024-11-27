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
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DiscountService> _logger;
        private readonly IContextService _contextService;

        public DiscountService(IDiscountDbContext dbContext, IStripeService stripeService, IEnumerable<ISieveProcessor> sieveProcessors, IMessageBroker messageBroker,
            ILogger<DiscountService> logger, IContextService contextService)
        {
            _dbContext = dbContext;
            _stripeService = stripeService;
            _sieveProcessor = sieveProcessors.First(s => s.GetType() == typeof(DiscountsModuleSieveProcessor));
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
        }

        public async Task<PagedResult<DiscountBrowseDto>> BrowseDiscountsAsync(string stripeCouponId, SieveModel model)
        {
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
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
            var pagedResult = new PagedResult<DiscountBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task CreateAsync(string stripeCouponId, DiscountCreateDto dto)
        {
            var discount = await _dbContext.Discounts.SingleOrDefaultAsync(d => d.Code == dto.Code);
            if (discount is not null)
            {
                throw new DiscountCodeAlreadyInUseException(dto.Code);
            }
            var coupon = await _dbContext.Coupons.SingleOrDefaultAsync(c => c.StripeCouponId == stripeCouponId);
            if(coupon is null)
            {
                throw new CouponNotFoundException(stripeCouponId);
            }
            var stripePromotionCodeId = await _stripeService.CreateDiscountAsync(stripeCouponId, dto);
            coupon.AddDiscount(new Discount(dto.Code, stripePromotionCodeId, dto.EndingDate));
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Discount: {discount} was created for coupon: {coupon} by {username}:{userId}.", dto, coupon, _contextService.Identity!.Username, _contextService.Identity!.Id);
        }

        public async Task ActivateAsync(string code)
        {
            var discount = await GetDiscountOrThrowIfNull(code);
            if(discount.IsActive is true)
            {
                throw new DiscountAlreadyActivated(code);
            }
            await _stripeService.ActivateDiscountAsync(discount.StripePromotionCodeId);
            discount.Activate();
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Discount: {discount} was activated by {username}:{userId}.", discount, _contextService.Identity!.Username, _contextService.Identity!.Id);
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
            discount.Deactive();
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Discount: {discount} was deactivated by {username}:{userId}.", discount, _contextService.Identity!.Username, _contextService.Identity!.Id);
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
    }
}
