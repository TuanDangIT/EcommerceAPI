using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Events.Externals.Handlers
{
    internal class OrderCancelledHandler : IEventHandler<OrderCancelled>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuctionRepository _auctionRepository;

        public OrderCancelledHandler(IProductRepository productRepository, IAuctionRepository auctionRepository)
        {
            _productRepository = productRepository;
            _auctionRepository = auctionRepository;
        }
        public async Task HandleAsync(OrderCancelled @event)
        {
            var products = await _productRepository.GetAllThatContainsInArrayAsync(@event.Products.Select(p => p.Id).ToArray());
            var auctions = await _auctionRepository.GetAllThatContainsInArrayAsync(@event.Products.Select(p => p.Id).ToArray());
            if (!products.Any() || products.Count() != @event.Products.Count())
            {
                throw new EventHandlerException("Product array was empty or not equal to event's product array.");
            }
            for (int i = 0; i < products.Count(); i++)
            {
                var product = products.ElementAt(i);
                var auction = auctions.SingleOrDefault(a => a.Id == product.Id);
                var soldProduct = @event.Products.Single(p => p.Id == product.Id);
                var soldQuantity = soldProduct.Quantity;
                if (product.HasQuantity && soldQuantity is not null)
                {
                    product.IncreaseQuantity((int)soldQuantity);
                }
                if(auction is not null && auction.HasQuantity && soldQuantity is not null)
                {
                    auction.IncreaseQuantity((int)soldQuantity);
                }
            }
            await _productRepository.UpdateAsync();
        }
    }
}
