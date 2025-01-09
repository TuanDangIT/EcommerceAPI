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
    internal class ProductQuantityChangedHandler : IDomainEventHandler<ProductQuantityChanged>
    {
        private readonly IAuctionRepository _auctionRepository;

        public ProductQuantityChangedHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }
        public async Task HandleAsync(ProductQuantityChanged @event)
        {
            var auction = await _auctionRepository.GetAsync(@event.ProductId) ?? 
                throw new AuctionNotFoundException(@event.ProductId);
            auction.DecreaseQuantity(@event.Quantity);
            await _auctionRepository.UpdateAsync();
        }
    }
}
