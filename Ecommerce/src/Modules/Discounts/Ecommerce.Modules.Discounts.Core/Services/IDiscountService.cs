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
        Task<IEnumerable<DiscountBrowseDto>> BrowseDiscountsAsync(int couponId, CancellationToken cancellationToken = default);
        Task CreateAsync(int couponId, DiscountCreateDto dto, CancellationToken cancellationToken = default);
        Task ActivateAsync(int couponId, int discountId, CancellationToken cancellationToken = default);
        Task DeactivateAsync(int couponId, int discountId, CancellationToken cancellationToken = default);
    }
}
