using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Services
{
    public interface ICouponService
    {
        Task CreateAsync(NominalCouponCreateDto dto);
        Task CreateAsync(PercentageCouponCreateDto dto);
        Task UpdateNameAsync(string stripeCouponId, CouponUpdateNameDto dto);
        Task DeleteAsync(string stripeCouponId);
        Task<PagedResult<NominalCouponBrowseDto>> BrowseNominalCouponsAsync(SieveModel model);
        Task<PagedResult<PercentageCouponBrowseDto>> BrowsePercentageCouponsAsync(SieveModel model);
    }
}
