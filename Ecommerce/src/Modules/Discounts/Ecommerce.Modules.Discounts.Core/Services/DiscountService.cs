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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Sieve.Extensions.MethodInfoExtended;

namespace Ecommerce.Modules.Discounts.Core.Services
{
    internal class DiscountService : IDiscountService
    {
        private readonly IDiscountDbContext _dbContext;
        private readonly IPaymentProcessorService _paymentProcessorService;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<DiscountService> _logger;
        private readonly IContextService _contextService;
        private readonly TimeProvider _timeProvider;

        public DiscountService(IDiscountDbContext dbContext, IPaymentProcessorService paymentProcessorService, IEnumerable<ISieveProcessor> sieveProcessors, IMessageBroker messageBroker,
            ILogger<DiscountService> logger, IContextService contextService, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _paymentProcessorService = paymentProcessorService;
            _sieveProcessor = sieveProcessors.First(s => s.GetType() == typeof(DiscountsModuleSieveProcessor));
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
            _timeProvider = timeProvider;
        }

        public async Task<PagedResult<DiscountBrowseDto>> BrowseDiscountsAsync(string stripeCouponId, SieveModel model, CancellationToken cancellationToken = default)
        {
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
            var coupon = await _dbContext.Coupons
                .Select(c => new { c.Id, c.StripeCouponId })
                .SingleOrDefaultAsync(c => c.StripeCouponId == stripeCouponId) ?? 
                throw new CouponNotFoundException(stripeCouponId);
            var discounts = _dbContext.Discounts
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, discounts)
                .Where(d => d.CouponId == coupon.Id)
                .Select(d => d.AsBrowseDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(model, discounts, applyPagination: false, applySorting: false)
                .Where(d => d.CouponId == coupon.Id)
                .CountAsync(cancellationToken);
            var pagedResult = new PagedResult<DiscountBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task CreateAsync(string stripeCouponId, DiscountCreateDto dto, CancellationToken cancellationToken = default)
        {
            var discount = await _dbContext.Discounts
                .Select(d => d.Code)
                .SingleOrDefaultAsync(code => code == dto.Code, cancellationToken);
            if (discount is not null)
            {
                throw new DiscountCodeAlreadyInUseException(dto.Code);
            }
            var coupon = await _dbContext.Coupons.SingleOrDefaultAsync(c => c.StripeCouponId == stripeCouponId, cancellationToken) ?? 
                throw new CouponNotFoundException(stripeCouponId);
            var stripePromotionCodeId = await _paymentProcessorService.CreateDiscountAsync(stripeCouponId, dto, cancellationToken);
            coupon.AddDiscount(new Discount(dto.Code, stripePromotionCodeId, dto.EndingDate, _timeProvider.GetUtcNow().DateTime));
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Discount with given details: {@discount} was created for coupon: {coupon} by {@user}.", dto, coupon.Id, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }

        public async Task ActivateAsync(string code, CancellationToken cancellationToken = default)
        {
            var discount = await GetDiscountOrThrowIfNullAsync(code, cancellationToken, d => d.Coupon);
            if(discount.IsActive is true)
            {
                throw new DiscountAlreadyActivated(code);
            }
            await _paymentProcessorService.ActivateDiscountAsync(discount.StripePromotionCodeId, cancellationToken);
            discount.Activate();
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Discount: {discountId} was activated by {@user}.", discount.Id, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
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

        public async Task DeactivateAsync(string code, CancellationToken cancellationToken = default)
        {
            var discount = await GetDiscountOrThrowIfNullAsync(code, cancellationToken);
            if(discount.IsActive is false)
            {
                throw new DiscountAlreadyDeactivated(code); 
            }
            await _paymentProcessorService.DeactivateDiscountAsync(discount.StripePromotionCodeId, cancellationToken);
            discount.Deactive();
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Discount: {discountId} was deactivated by {@user}.", discount.Id,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            await _messageBroker.PublishAsync(new DiscountDeactivated(discount.Code));
        }
        private async Task<Discount> GetDiscountOrThrowIfNullAsync(string code, CancellationToken cancellationToken = default,
            params Expression<Func<Discount, object?>>[] includes)
        {
            var discounts = _dbContext.Discounts
                .AsQueryable();
            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    discounts = discounts.Include(include);
                }
            }
            var discount = await discounts
                .SingleOrDefaultAsync(d => d.Code == code, cancellationToken) ?? 
                throw new DiscountNotFoundException(code);
            return discount;
        }
    }
}
