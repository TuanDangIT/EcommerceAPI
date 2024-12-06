using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Services
{
    public interface IDiscountService
    {
        Task<PagedResult<DiscountBrowseDto>> BrowseDiscountsAsync(string stripeCouponId, SieveModel model, CancellationToken cancellationToken = default);
        Task CreateAsync(string stripeCouponId, DiscountCreateDto dto, CancellationToken cancellationToken = default);
        Task ActivateAsync(string code, CancellationToken cancellationToken = default);
        Task DeactivateAsync(string code, CancellationToken cancellationToken = default);
    }
}
