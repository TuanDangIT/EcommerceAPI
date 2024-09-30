using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Exceptions;
using Ecommerce.Modules.Discounts.Core.Services.Externals;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
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
        private readonly TimeProvider _timeProvider;

        public CouponService(IDiscountDbContext dbContext, IStripeService stripeService, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _stripeService = stripeService;
            _timeProvider = timeProvider;
        }
        public Task<PagedResult<NominalCouponBrowseDto>> BrowseNominalCouponsAsync(SieveModel model)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<PercentageCouponBrowseDto>> BrowsePercentageCouponsAsync(SieveModel model)
        {
            throw new NotImplementedException();
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
            await _stripeService.DeleteCouponAsync(stripeCouponId);
            await _dbContext.Coupons
                .Where(c => c.StripeCouponId == stripeCouponId)
                .ExecuteDeleteAsync();
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
