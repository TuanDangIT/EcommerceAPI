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
        //Task CreateAsync(Offer offer);
        Task AcceptAsync(int offerId);
        Task RejectAsync(int offerId);  
        Task DeleteAsync(int offerId);
        Task<PagedResult<OfferBrowseDto>> BrowseAsync(SieveModel model);
        Task<OfferDetailsDto> GetAsync(int offerId);
    }
}
