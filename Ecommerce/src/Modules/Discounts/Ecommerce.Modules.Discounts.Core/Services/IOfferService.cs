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
    public interface IOfferService
    {
        Task AcceptAsync(int offerId, CancellationToken cancellationToken = default);
        Task RejectAsync(int offerId, CancellationToken cancellationToken = default);  
        Task DeleteAsync(int offerId, CancellationToken cancellationToken = default);
        Task<PagedResult<OfferBrowseDto>> BrowseAsync(SieveModel model, CancellationToken cancellationToken = default);
        Task<OfferDetailsDto> GetAsync(int offerId, CancellationToken cancellationToken = default);
    }
}
