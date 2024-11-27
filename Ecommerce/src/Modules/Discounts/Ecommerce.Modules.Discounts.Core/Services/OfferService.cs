using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.DAL.Mappings;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Ecommerce.Modules.Discounts.Core.Events;
using Ecommerce.Modules.Discounts.Core.Exceptions;
using Ecommerce.Modules.Discounts.Core.Sieve;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<OfferService> _logger;
        private readonly IContextService _contextService;

        public OfferService(IDiscountDbContext dbContext, IEnumerable<ISieveProcessor> sieveProcessors, IMessageBroker messageBroker, TimeProvider timeProvider,
            ILogger<OfferService> logger, IContextService contextService)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessors.First(s => s.GetType() == typeof(DiscountsModuleSieveProcessor));
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task AcceptAsync(int offerId)
        {
            var offer = await _dbContext.Offers
                .SingleOrDefaultAsync(o => o.Id == offerId) ??
                throw new OfferNotFoundException(offerId);
            var expiresAt = _timeProvider.GetUtcNow().UtcDateTime + TimeSpan.FromDays(7);
            offer.Accept(expiresAt);
            var code = GenerateRandomCode();
            offer.SetCode(code);
            await _messageBroker
                .PublishAsync(new OfferAccepted(offer.Id, offer.CustomerId, offer.SKU, offer.ProductName, code, offer.OfferedPrice, offer.OldPrice, expiresAt));
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Offer: {offer} with code: {code} was accepted by {username}:{userId}.", offer, code, _contextService.Identity!.Username, _contextService.Identity!.Id);
        }
        public async Task RejectAsync(int offerId)
        {
            var offer = await _dbContext.Offers
                .SingleOrDefaultAsync(o => o.Id == offerId) ??
                throw new OfferNotFoundException(offerId);
            offer.Reject();
            if(offer.Code is null)
            {
                await _messageBroker
                    .PublishAsync(new OfferRejected(offer.Id, offer.CustomerId, offer.SKU, offer.ProductName, offer.OfferedPrice, offer.OldPrice));
            }
            else
            {
                await _messageBroker
                    .PublishAsync(new OfferRejected(offer.Id, offer.CustomerId, offer.Code, offer.SKU, offer.ProductName, offer.OfferedPrice, offer.OldPrice));
            }
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Offer: {offer} was rejected by {username}:{userId}.", offer, _contextService.Identity!.Username, _contextService.Identity!.Id);
        }

        public async Task<PagedResult<OfferBrowseDto>> BrowseAsync(SieveModel model)
        {
            if (model.PageSize is null || model.Page is null)
            {
                throw new PaginationException();
            }
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
            var pagedResult = new PagedResult<OfferBrowseDto>(dtos, totalCount, model.PageSize.Value, model.Page.Value);
            return pagedResult;
        }

        public async Task DeleteAsync(int offerId) 
        {
            await _dbContext.Offers.Where(o => o.Id == offerId).ExecuteDeleteAsync();
            _logger.LogInformation("Offer: {offerId} was deleted by {username}:{userId}", offerId, _contextService.Identity!.Username, _contextService.Identity!.Id);
        }

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
