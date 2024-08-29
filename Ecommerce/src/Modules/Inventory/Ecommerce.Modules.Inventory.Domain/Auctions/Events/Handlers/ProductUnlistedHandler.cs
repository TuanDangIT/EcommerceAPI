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
    public sealed class ProductUnlistedHandler : IDomainEventHandler<ProductUnlisted>
    {
        private readonly IAuctionRepository _auctionRepository;

        public ProductUnlistedHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }
        async Task IDomainEventHandler<ProductUnlisted>.HandleAsync(ProductUnlisted @event)
        {
            await _auctionRepository.DeleteManyAsync(@event.ProductIds.ToArray());
        }
    }
}
