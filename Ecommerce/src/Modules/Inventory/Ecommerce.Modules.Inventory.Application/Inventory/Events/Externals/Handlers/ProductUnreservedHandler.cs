using Ecommerce.Modules.Inventory.Application.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
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
    internal class ProductUnreservedHandler : IEventHandler<ProductUnreserved>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuctionRepository _auctionRepository;

        public ProductUnreservedHandler(IProductRepository productRepository, IAuctionRepository auctionRepository)
        {
            _productRepository = productRepository;
            _auctionRepository = auctionRepository;
        }
        public async Task HandleAsync(ProductUnreserved @event)
        {
            var product = await _productRepository.GetAsync(@event.ProductId) ??
                    throw new ProductNotFoundException(@event.ProductId);
            var auction = await _auctionRepository.GetAsync(@event.ProductId) ??
                throw new AuctionNotFoundException(@event.ProductId);
            if (product.HasQuantity)
            {
                product.Unreserve(@event.Quantity);
                auction.IncreaseQuantity(@event.Quantity);
                await _productRepository.UpdateAsync();
            }
        }
    }
}
