using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Shared.Infrastructure.Pagination;
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
        Task CreateAsync(NominalDiscountCreateDto dto);
        Task CreateAsync(PercentageDiscountCreateDto dto);
        Task DeleteAsync(string code);
        Task<PagedResult<NominalDiscountBrowseDto>> BrowseNominalDiscountsAsync(SieveModel model);
        Task<PagedResult<PercentageDiscountBrowseDto>> BrowsePercentageDiscountsAsync(SieveModel model);
    }
}
