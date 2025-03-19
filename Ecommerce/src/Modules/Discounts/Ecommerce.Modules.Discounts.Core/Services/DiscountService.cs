using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.DAL.Mappings;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Events;
using Ecommerce.Modules.Discounts.Core.Exceptions;
using Ecommerce.Modules.Discounts.Core.Services.Externals;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

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
        private readonly IOptions<SieveOptions> _sieveOptions;

        public DiscountService(IDiscountDbContext dbContext, IPaymentProcessorService paymentProcessorService, [FromKeyedServices("discounts-sieve-processor")]ISieveProcessor sieveProcessor, 
            IMessageBroker messageBroker, ILogger<DiscountService> logger, IContextService contextService, TimeProvider timeProvider, IOptions<SieveOptions> sieveOptions)
        {
            _dbContext = dbContext;
            _paymentProcessorService = paymentProcessorService;
            _sieveProcessor = sieveProcessor;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
            _timeProvider = timeProvider;
            _sieveOptions = sieveOptions;
        }

        public async Task<IEnumerable<DiscountBrowseDto>> BrowseDiscountsAsync(int couponId, CancellationToken cancellationToken = default)
            => await _dbContext.Discounts
                .Where(d => d.CouponId == couponId)
                .Select(d => d.AsBrowseDto())
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        public async Task CreateAsync(int couponId, DiscountCreateDto dto, CancellationToken cancellationToken = default)
        {
            var discount = await _dbContext.Discounts
                .Select(d => d.Code)
                .FirstOrDefaultAsync(code => code == dto.Code, cancellationToken);
            if (discount is not null)
            {
                throw new DiscountCodeAlreadyInUseException(dto.Code);
            }
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == couponId, cancellationToken) ?? 
                throw new CouponNotFoundException(couponId);
            var stripePromotionCodeId = await _paymentProcessorService.CreateDiscountAsync(coupon.StripeCouponId, dto, cancellationToken);
            var expiresDate = dto.ExpiresDate is null ? dto.ExpiresDate :  DateTime.SpecifyKind((DateTime)dto.ExpiresDate, DateTimeKind.Utc);
            coupon.AddDiscount(new Entities.Discount(dto.Code, stripePromotionCodeId, dto.RequiredCartTotalValue ?? 0, expiresDate, _timeProvider.GetUtcNow().DateTime));
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Discount with given details: {@discount} was created for coupon: {coupon} by {@user}.", dto, coupon.Id, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }

        public async Task ActivateAsync(int couponId, int discountId, CancellationToken cancellationToken = default)
        {
            var coupon = await _dbContext.Coupons
                .Include(c => c.Discounts)
            .FirstOrDefaultAsync(c => c.Id == couponId, cancellationToken) ?? throw new CouponNotFoundException(couponId);
            var discount = coupon.Discounts
                .FirstOrDefault(d => d.Id == discountId) ?? throw new DiscountNotFoundException(discountId);
            if (discount.ExpiresAt < _timeProvider.GetUtcNow().DateTime)
            {
                throw new CannotActivateExpiredDiscountException(discount.Code);
            }
            if(discount.IsActive is true)
            {
                throw new DiscountAlreadyActivated(discount.Code);
            }
            await _paymentProcessorService.ActivateDiscountAsync(discount.StripePromotionCodeId, cancellationToken);
            discount.Activate();
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Discount: {discountId} was activated by {@user}.", discount.Id, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            switch (coupon)
            {
                case NominalCoupon nominalCoupon:
                    await _messageBroker.PublishAsync(
                        new DiscountActivated(discount.Code, nominalCoupon.Type.ToString(), discount.StripePromotionCodeId, nominalCoupon.NominalValue, discount.RequiredCartTotalValue, discount.ExpiresAt));
                    break;
                case PercentageCoupon percentageCoupon:
                    await _messageBroker.PublishAsync(
                        new DiscountActivated(discount.Code, percentageCoupon.Type.ToString(), discount.StripePromotionCodeId, percentageCoupon.Percent, discount.RequiredCartTotalValue, discount.ExpiresAt));
                    break;
            }
        }

        public async Task DeactivateAsync(int couponId, int discountId, CancellationToken cancellationToken = default)
        {
            var coupon = await _dbContext.Coupons
                .Include(c => c.Discounts)
                .FirstOrDefaultAsync(c => c.Id == couponId, cancellationToken) ?? throw new CouponNotFoundException(couponId);
            var discount = coupon.Discounts
                .FirstOrDefault(d => d.Id == discountId) ?? throw new DiscountNotFoundException(discountId);
            if(discount.IsActive is false)
            {
                throw new DiscountAlreadyDeactivated(discount.Code); 
            }
            await _paymentProcessorService.DeactivateDiscountAsync(discount.StripePromotionCodeId, cancellationToken);
            discount.Deactive();
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Discount: {discountId} was deactivated by {@user}.", discount.Id,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            await _messageBroker.PublishAsync(new DiscountDeactivated(discount.Code));
        }
        //private async Task<Discount> GetDiscountOrThrowIfNullAsync(int discountId, CancellationToken cancellationToken = default,
        //    params Expression<Func<Discount, object?>>[] includes)
        //{
        //    var discounts = _dbContext.Discounts
        //        .AsQueryable();
        //    if (includes is not null)
        //    {
        //        foreach (var include in includes)
        //        {
        //            discounts = discounts.Include(include);
        //        }
        //    }
        //    var discount = await discounts
        //        .FirstOrDefaultAsync(d => d.Id == discountId, cancellationToken) ??
        //        throw new DiscountNotFoundException(discountId);
        //    return discount;
        //}
    }
}
