﻿using Ecommerce.Modules.Discounts.Core.DAL;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<SieveOptions> _sieveOptions;
        private readonly ISieveProcessor _sieveProcessor;

        public CouponService(IDiscountDbContext dbContext, IPaymentProcessorService paymentProcessorService, IMessageBroker messageBroker, [FromKeyedServices("discounts-sieve-processor")] ISieveProcessor sieveProcessor,
            ILogger<CouponService> logger, IContextService contextService, IOptions<SieveOptions> sieveOptions)
        {
            _dbContext = dbContext;
            _paymentProcessorService = paymentProcessorService;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
            _sieveOptions = sieveOptions;
            _sieveProcessor = sieveProcessor;
        }
        public async Task<PagedResult<NominalCouponBrowseDto>> BrowseNominalCouponsAsync(SieveModel model, CancellationToken cancellationToken = default)
        {
            if (model.Page is null || model.Page <= 0)
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
            int pageSize = _sieveOptions.Value.DefaultPageSize;
            if (model.PageSize is not null || model.PageSize <= 0)
            {
                pageSize = model.PageSize.Value;
            }
            var pagedResult = new PagedResult<NominalCouponBrowseDto>(dtos, totalCount, pageSize, model.Page.Value);
            return pagedResult;
        }

        public async Task<PagedResult<PercentageCouponBrowseDto>> BrowsePercentageCouponsAsync(SieveModel model, CancellationToken cancellationToken = default)
        {
            if (model.Page is null || model.Page <= 0)
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
            int pageSize = _sieveOptions.Value.DefaultPageSize;
            if (model.PageSize is not null || model.PageSize <= 0)
            {
                pageSize = model.PageSize.Value;
            }
            var pagedResult = new PagedResult<PercentageCouponBrowseDto>(dtos, totalCount, pageSize, model.Page.Value);
            return pagedResult;
        }

        public async Task<int> CreateAsync(NominalCouponCreateDto dto, CancellationToken cancellationToken = default)
        {
            string? stripeCouponId = null;
            try
            {
                stripeCouponId = await _paymentProcessorService.CreateCouponAsync(dto, cancellationToken);
                var coupon = new NominalCoupon(dto.Name, dto.NominalValue, stripeCouponId);
                await _dbContext.Coupons.AddAsync(coupon);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Nominal coupon with given details: {@coupon} was created by {@user}.", dto,
                    new { _contextService.Identity!.Username, _contextService.Identity!.Id });
                return coupon.Id;
            }
            catch(Exception)
            {
                if(!string.IsNullOrEmpty(stripeCouponId))
                {
                    await _paymentProcessorService.DeleteCouponAsync(stripeCouponId, cancellationToken);
                }
                throw;
            }
        }

        public async Task<int> CreateAsync(PercentageCouponCreateDto dto, CancellationToken cancellationToken = default)
        {
            string? stripeCouponId = null;
            try
            {
                stripeCouponId = await _paymentProcessorService.CreateCouponAsync(dto, cancellationToken);
                var coupon = new PercentageCoupon(dto.Name, dto.Percent, stripeCouponId);
                await _dbContext.Coupons.AddAsync(coupon);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Percentage coupon with given details: {@coupon} was created by {@user}.", dto,
                    new { _contextService.Identity!.Username, _contextService.Identity!.Id });
                return coupon.Id;
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(stripeCouponId))
                {
                    await _paymentProcessorService.DeleteCouponAsync(stripeCouponId, cancellationToken);
                }
                throw;
            }
        }

        public async Task DeleteAsync(int couponId, CancellationToken cancellationToken = default)
        {
            var coupon = await _dbContext.Coupons
                .Include(c => c.Discounts)
                .FirstOrDefaultAsync(c => c.Id == couponId, cancellationToken) ?? 
                throw new CouponNotFoundException(couponId);
            await _paymentProcessorService.DeleteCouponAsync(coupon.StripeCouponId, cancellationToken);
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

        public async Task UpdateNameAsync(int couponId, CouponUpdateNameDto dto, CancellationToken cancellationToken = default)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == couponId, cancellationToken) ?? 
                throw new CouponNotFoundException(couponId);
            coupon.ChangeName(dto.Name);
            await _paymentProcessorService.UpdateCouponName(coupon.StripeCouponId, dto.Name, cancellationToken);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Coupon: {couponId} was updated with new details: {@updatingDetails} by {@user}.", coupon.Id, dto,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
