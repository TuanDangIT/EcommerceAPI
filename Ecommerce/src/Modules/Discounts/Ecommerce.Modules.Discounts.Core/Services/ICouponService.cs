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
        Task CreateAsync(NominalCouponCreateDto dto, CancellationToken cancellationToken = default);
        Task CreateAsync(PercentageCouponCreateDto dto, CancellationToken cancellationToken = default);
        Task UpdateNameAsync(int couponId, CouponUpdateNameDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int couponId, CancellationToken cancellationToken = default);
        Task<PagedResult<NominalCouponBrowseDto>> BrowseNominalCouponsAsync(SieveModel model, CancellationToken cancellationToken = default);
        Task<PagedResult<PercentageCouponBrowseDto>> BrowsePercentageCouponsAsync(SieveModel model, CancellationToken cancellationToken = default);
    }
}
