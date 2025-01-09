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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly IPaymentProcessorService _paymentProcessorService;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<CouponService> _logger;
        private readonly IContextService _contextService;
        private readonly ISieveProcessor _sieveProcessor;

        public CouponService(IDiscountDbContext dbContext, IPaymentProcessorService paymentProcessorService, IMessageBroker messageBroker, IEnumerable<ISieveProcessor> sieveProcessors,
            ILogger<CouponService> logger, IContextService contextService)
        {
            _dbContext = dbContext;
            _paymentProcessorService = paymentProcessorService;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
            _sieveProcessor = sieveProcessors.First(s => s.GetType() == typeof(DiscountsModuleSieveProcessor));
        }
        public async Task<PagedResult<NominalCouponBrowseDto>> BrowseNominalCouponsAsync(SieveModel model, CancellationToken cancellationToken = default)
        {
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
            var coupons = _dbContext.Coupons
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, coupons)
                .OfType<NominalCoupon>()
                .Select(nd => nd.AsNominalBrowseDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(model, coupons, applyPagination: false, applySorting: false)
                .OfType<NominalCoupon>()
                .CountAsync(cancellationToken);
            var pagedResult = new PagedResult<NominalCouponBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task<PagedResult<PercentageCouponBrowseDto>> BrowsePercentageCouponsAsync(SieveModel model, CancellationToken cancellationToken = default)
        {
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
            var coupons = _dbContext.Coupons
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, coupons)
                .OfType<PercentageCoupon>()
                .Select(nd => nd.AsPercentageBrowseDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(model, coupons, applyPagination: false, applySorting: false)
                .OfType<PercentageCoupon>()
                .CountAsync(cancellationToken);
            var pagedResult = new PagedResult<PercentageCouponBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task CreateAsync(NominalCouponCreateDto dto, CancellationToken cancellationToken = default)
        {
            var stripeCouponId = await _paymentProcessorService.CreateCouponAsync(dto, cancellationToken);
            await _dbContext.Coupons.AddAsync(new NominalCoupon(dto.Name, dto.NominalValue, stripeCouponId));
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Nominal coupon with given details: {@coupon} was created by {@user}.", dto, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }

        public async Task CreateAsync(PercentageCouponCreateDto dto, CancellationToken cancellationToken = default)
        {
            var stripeCouponId = await _paymentProcessorService.CreateCouponAsync(dto, cancellationToken);
            await _dbContext.Coupons.AddAsync(new PercentageCoupon(dto.Name, dto.Percent, stripeCouponId));
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Percentage coupon with given details: {@coupon} was created by {@user}.", dto,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }

        public async Task DeleteAsync(string stripeCouponId, CancellationToken cancellationToken = default)
        {
            var coupon = await _dbContext.Coupons
                .Include(c => c.Discounts)
                .SingleOrDefaultAsync(c => c.StripeCouponId == stripeCouponId, cancellationToken) ?? 
                throw new CouponNotFoundException(stripeCouponId);
            await _paymentProcessorService.DeleteCouponAsync(stripeCouponId, cancellationToken);
            if (coupon.HasDiscounts)
            {
                foreach(var discount in coupon.Discounts)
                {
                    await _messageBroker.PublishAsync(new DiscountDeactivated(discount.Code));
                }
            }
            _dbContext.Coupons.Remove(coupon);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Coupon: {couponId} was deleted by {@user}.", coupon.Id,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }

        public async Task UpdateNameAsync(string stripeCouponId, CouponUpdateNameDto dto, CancellationToken cancellationToken = default)
        {
            var coupon = await _dbContext.Coupons.SingleOrDefaultAsync(c => c.StripeCouponId == stripeCouponId, cancellationToken) ?? 
                throw new CouponNotFoundException(stripeCouponId);
            coupon.ChangeName(dto.Name);
            await _paymentProcessorService.UpdateCouponName(stripeCouponId, dto.Name, cancellationToken);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Coupon: {couponId} was updated with new details: {@updatingDetails} by {@user}.", coupon.Id, dto,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
