using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Events.Externals.Handlers
{
    internal class CustomerPlacedOrderHandler : IEventHandler<CustomerPlacedOrder>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuctionRepository _auctionRepository;

        public CustomerPlacedOrderHandler(IProductRepository productRepository, IAuctionRepository auctionRepository)
        {
            _productRepository = productRepository;
            _auctionRepository = auctionRepository;
        }
        public async Task HandleAsync(CustomerPlacedOrder @event)
        {
            var products = await _productRepository.GetAllThatContainsInArrayAsync(@event.Products.Select(p => p.Id).ToArray());
            var auctions = await _auctionRepository.GetAllThatContainsInArrayAsync(@event.Products.Select(p => p.Id).ToArray());
            if(!products.Any() || !auctions.Any() || products.Count() != @event.Products.Count() || auctions.Count() != @event.Products.Count())
            {
                throw new ArgumentException();
            }
            var auctionsToDelete = new List<Guid>();
            for(int i=0; i < products.Count(); i++)
            {
                var product = products.ElementAt(i);
                var auction = auctions.Single(a => a.Id == product.Id);
                var soldProduct = @event.Products.Single(p => p.Id == product.Id);
                var soldQuantity = soldProduct.Quantity;
                if (product.HasQuantity && auction.HasQuantity && soldQuantity is not null)
                {
                    product.Purchase((int)soldQuantity!);
                    auction.DecreaseQuantity((int)soldQuantity);
                    if (soldQuantity == auction.Quantity)
                    {
                        auctionsToDelete.Add(auction.Id);
                    }
                    else
                    {
                        auction.DecreaseQuantity((int)soldQuantity);
                    }
                }
            }
            //if(auctionsToDelete.Count > 0)
            //{
            //    await _auctionRepository.UnlistManyAsync([.. auctionsToDelete]);
            //    await _productRepository.UpdateListedFlagAsync([.. auctionsToDelete], false);
            //}
            await _productRepository.UpdateAsync();
        }
    }
}
