using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Events.Externals.Handlers
{
    internal class ProductRemovedFromOrderHandler : IEventHandler<ProductRemovedFromOrder>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuctionRepository _auctionRepository;

        public ProductRemovedFromOrderHandler(IProductRepository productRepository, IAuctionRepository auctionRepository)
        {
            _productRepository = productRepository;
            _auctionRepository = auctionRepository;
        }
        public async Task HandleAsync(ProductRemovedFromOrder @event)
        {
            var product = await _productRepository.GetAsync(@event.ProductId);
            var auction = await _auctionRepository.GetAsync(@event.ProductId);

            if (product is not null && product.HasQuantity)
            {
                product.IncreaseQuantity(@event.Quantity);
            }

            if (auction is not null && auction.HasQuantity)
            {
                auction.IncreaseQuantity(@event.Quantity);
            }

            await _productRepository.UpdateAsync();
        }
    }
}
