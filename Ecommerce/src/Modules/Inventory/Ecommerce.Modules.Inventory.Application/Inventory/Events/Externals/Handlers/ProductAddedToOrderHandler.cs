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
    internal class ProductAddedToOrderHandler : IEventHandler<ProductAddedToOrder>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuctionRepository _auctionRepository;

        public ProductAddedToOrderHandler(IProductRepository productRepository, IAuctionRepository auctionRepository)
        {
            _productRepository = productRepository;
            _auctionRepository = auctionRepository;
        }
        public async Task HandleAsync(ProductAddedToOrder @event)
        {
            var product = await _productRepository.GetAsync(@event.ProductId);
            var auction = await _auctionRepository.GetAsync(@event.ProductId);

            if (product is not null && product.HasQuantity)
            {
                product.DecreaseQuantity(@event.Quantity);
            }

            if (auction is not null && auction.HasQuantity)
            {
                auction.DecreaseQuantity(@event.Quantity);
            }

            await _productRepository.UpdateAsync();
        }
    }
}
