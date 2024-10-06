using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.DAL.Mappings;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Ecommerce.Modules.Discounts.Core.Events;
using Ecommerce.Modules.Discounts.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Services
{
    internal class OfferService : IOfferService
    {
        private readonly IDiscountDbContext _dbContext;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly IMessageBroker _messageBroker;
        private readonly TimeProvider _timeProvider;

        public OfferService(IDiscountDbContext dbContext, ISieveProcessor sieveProcessor, IMessageBroker messageBroker, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
        }
        public async Task AcceptAsync(int offerId)
        {
            var offer = await _dbContext.Offers
                .SingleOrDefaultAsync(o => o.Id == offerId) ??
                throw new OfferNotFoundException(offerId);
            offer.Accept(_timeProvider.GetUtcNow().UtcDateTime);
            //Event + mail
            await _messageBroker
                .PublishAsync(new OfferAccepted(offer.SKU, GenerateRandomCode(), offer.Price, offer.CustomerId, _timeProvider.GetUtcNow().UtcDateTime + TimeSpan.FromDays(7)));
            await _dbContext.SaveChangesAsync();
        }
        public async Task RejectAsync(int offerId)
        {
            var offer = await _dbContext.Offers
                .SingleOrDefaultAsync(o => o.Id == offerId) ??
                throw new OfferNotFoundException(offerId);
            offer.Reject(_timeProvider.GetUtcNow().UtcDateTime);
            //Wysłanie maila
            //Ewentualne skasowanie??
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedResult<OfferBrowseDto>> BrowseAsync(SieveModel model)
        {
            var coupons = _dbContext.Offers
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(model, coupons)
                .Select(o => o.AsBrowseDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(model, coupons, applyPagination: false, applySorting: false)
                .CountAsync();
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<OfferBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        //public async Task CreateAsync(Offer offer)
        //{
        //    await _dbContext.Offers.AddAsync(offer);
        //    await _dbContext.SaveChangesAsync();
        //}

        public async Task DeleteAsync(int offerId)
            => await _dbContext.Offers.Where(o => o.Id == offerId).ExecuteDeleteAsync();

        public async Task<OfferDetailsDto> GetAsync(int offerId)
        {
            var offer = await _dbContext.Offers
                .SingleOrDefaultAsync(o => o.Id == offerId) ?? 
                throw new OfferNotFoundException(offerId);
            return offer.AsDetailsDto();
        }
        private static string GenerateRandomCode()
            => Guid.NewGuid().ToString();

    }
}
