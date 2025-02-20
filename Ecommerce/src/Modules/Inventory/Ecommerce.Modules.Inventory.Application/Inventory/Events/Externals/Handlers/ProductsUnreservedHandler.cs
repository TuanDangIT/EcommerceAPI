using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Events.Externals.Handlers
{
    internal class ProductsUnreservedHandler : IEventHandler<ProductsUnreserved>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuctionRepository _auctionRepository;

        public ProductsUnreservedHandler(IProductRepository productRepository, IAuctionRepository auctionRepository)
        {
            _productRepository = productRepository;
            _auctionRepository = auctionRepository;
        }
        public async Task HandleAsync(ProductsUnreserved @event)
        {
            var products = await _productRepository.GetAllThatContainsInArrayAsync(@event.Products.Select(p => p.Key).ToArray());
            var auctions = await _auctionRepository.GetAllThatContainsInArrayAsync(@event.Products.Select(p => p.Key).ToArray());
            foreach (var productKeyValuePair in @event.Products)
            {
                var product = products.SingleOrDefault(p => p.Id == productKeyValuePair.Key);
                var auction = auctions.SingleOrDefault(p => p.Id == productKeyValuePair.Key);
                if (product is null || !product.HasQuantity || auction is null || !auction.HasQuantity)
                {
                    continue;
                }
                product.Unreserve(productKeyValuePair.Value);
                auction.IncreaseQuantity(productKeyValuePair.Value);    
            }
            await _productRepository.UpdateAsync();
        }
    }
}
