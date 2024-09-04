using Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Modules.Inventory.Domain.Inventory.Events;
using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Events.Handlers
{
    public sealed class ProductsUnlistedHandler : IDomainEventHandler<ProductsUnlisted>
    {
        private readonly IAuctionRepository _auctionRepository;

        public ProductsUnlistedHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task HandleAsync(ProductsUnlisted @event)
        {
            var productIds = @event.ProductIds.ToArray();
            var auctions = await _auctionRepository.GetAllThatContainsInArrayAsync(productIds);
            if (!auctions.Any())
            {
                throw new ProductsCannotBeUnlistedMoreThanOnceException(auctions.Select(a => a.Id).ToArray());
            }
            var rowsChanged = await _auctionRepository.DeleteManyAsync(productIds);
            if (rowsChanged != productIds.Length)
            {
                throw new AuctionsNotDeletedException();
            }
        }
    }
}
