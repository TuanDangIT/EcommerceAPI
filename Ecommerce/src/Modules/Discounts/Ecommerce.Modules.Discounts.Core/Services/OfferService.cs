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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<SieveOptions> _sieveOptions;

        public OfferService(IDiscountDbContext dbContext, [FromKeyedServices("discounts-sieve-processor")] ISieveProcessor sieveProcessor, IMessageBroker messageBroker, TimeProvider timeProvider,
            ILogger<OfferService> logger, IContextService contextService, IOptions<SieveOptions> sieveOptions)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
            _logger = logger;
            _contextService = contextService;
            _sieveOptions = sieveOptions;
        }

        public async Task<PagedResult<OfferBrowseDto>> BrowseAsync(SieveModel model, CancellationToken cancellationToken = default)
        {
            if (model.Page is null)
            {
                throw new PaginationException();
            }
            var offers = _dbContext.Offers
                .AsNoTracking()
                .AsQueryable();
            if(_contextService.Identity!.Role == "Customer")
            {
                offers
                    .Where(o => o.CustomerId == _contextService.Identity!.Id)
                    .AsQueryable();
            }
            var dtos = await _sieveProcessor
                .Apply(model, offers)
                .Select(o => o.AsBrowseDto())
                .ToListAsync(cancellationToken);
            var totalCount = await _sieveProcessor
                .Apply(model, offers, applyPagination: false, applySorting: false)
                .CountAsync(cancellationToken);
            int pageSize = _sieveOptions.Value.DefaultPageSize;
            if (model.PageSize is not null)
            {
                pageSize = model.PageSize.Value;
            }
            var pagedResult = new PagedResult<OfferBrowseDto>(dtos, totalCount, pageSize, model.Page.Value);
            return pagedResult;
        }

        public async Task AcceptAsync(int offerId, CancellationToken cancellationToken = default)
        {
            var offer = await _dbContext.Offers
                .FirstOrDefaultAsync(o => o.Id == offerId, cancellationToken) ??
                throw new OfferNotFoundException(offerId);
            if(offer.Status  == OfferStatus.Accepted)
            {
                throw new OfferAlreadyAcceptedException(offerId);
            }
            var expiresAt = _timeProvider.GetUtcNow().UtcDateTime + TimeSpan.FromDays(7);
            offer.Accept(expiresAt, _timeProvider.GetUtcNow().DateTime);
            var code = GenerateRandomCode();
            offer.SetCode(code);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Offer: {offerId} with code: {code} was accepted by {@user}.", offer.Id, code, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            await _messageBroker
                .PublishAsync(new OfferAccepted(offer.Id, offer.CustomerId, offer.SKU, offer.ProductName, code, offer.OfferedPrice, offer.OldPrice, expiresAt));
        }
        public async Task RejectAsync(int offerId, CancellationToken cancellationToken = default)
        {
            var offer = await _dbContext.Offers
                .FirstOrDefaultAsync(o => o.Id == offerId, cancellationToken) ??
                throw new OfferNotFoundException(offerId);
            offer.Reject();
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Offer: {offerId} was rejected by {@user}.", offer.Id, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            if(!offer.HasCode)
            {
                await _messageBroker
                    .PublishAsync(new OfferRejected(offer.Id, offer.CustomerId, offer.SKU, offer.ProductName, offer.OfferedPrice, offer.OldPrice));
            }
            else
            {
                await _messageBroker
                    .PublishAsync(new OfferRejected(offer.Id, offer.CustomerId, offer.Code!, offer.SKU, offer.ProductName, offer.OfferedPrice, offer.OldPrice));
            }
        }

        public async Task DeleteAsync(int offerId, CancellationToken cancellationToken = default) 
        {
            await _dbContext.Offers.Where(o => o.Id == offerId).ExecuteDeleteAsync(cancellationToken);
            _logger.LogInformation("Offer: {offerId} was deleted by {@user}.", offerId, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
        public async Task DeleteManyAsync(IEnumerable<int> offerIds, CancellationToken cancellationToken = default)
        {
            await _dbContext.Offers
                .Where(o => offerIds.Contains(o.Id))
                .ExecuteDeleteAsync(cancellationToken);
            _logger.LogInformation("Offers: {offerIds} were deleted by {@user}.", offerIds, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
        public async Task<OfferDetailsDto> GetAsync(int offerId, CancellationToken cancellationToken = default)
            => await _dbContext.Offers
                .Where(o => o.Id == offerId)
                .Select(o => o.AsDetailsDto())
                .FirstOrDefaultAsync(cancellationToken) ??
                throw new OfferNotFoundException(offerId);
        private static string GenerateRandomCode()
            => Guid.NewGuid().ToString();

    }
}
