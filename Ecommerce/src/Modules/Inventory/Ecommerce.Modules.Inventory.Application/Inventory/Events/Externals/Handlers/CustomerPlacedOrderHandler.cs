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
            //foreach (var product in products)
            //{
            //    var productFromEvent = @event.Products.SingleOrDefault(p => p.Id == product.Id);
            //    if (productFromEvent is null)
            //    {
            //        //exception must be thrown here
            //        continue;
            //    }
            //    if (product.HasQuantity && productFromEvent.Quantity is not null)
            //    {
            //        product.DecreaseQuantity((int)productFromEvent.Quantity);
            //    }
            //}
            if(products.Count() == 0 || auctions.Count() == 0 || products.Count() != @event.Products.Count() || auctions.Count() != @event.Products.Count())
            {
                throw new Exception();
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
            if(auctionsToDelete.Count > 0)
            {
                await _auctionRepository.UnlistManyAsync([.. auctionsToDelete]);
                await _productRepository.UpdateListedFlagAsync([.. auctionsToDelete], false);
            }
            await _productRepository.UpdateAsync();




            //IEnumerable<Product>? productsFromEvent = JsonSerializer.Deserialize<IEnumerable<Product>>(JsonSerializer.Serialize(@event.Products));
            //if(@event.Products is null || !@event.Products.Any())
            //{
            //    return;
            //}
            //if(productsFromEvent is not null && productsFromEvent.Count() > 0)
            //{
            //    var products = await _productRepository.GetAllThatContainsInArrayAsync(productsFromEvent.Select(pfe => pfe.Id).ToArray());
            //    var auctions = await _auctionRepository.GetAllThatContainsInArrayAsync(productsFromEvent.Select(pfe => pfe.Id).ToArray());
            //    foreach (var product in products)
            //    {
            //        var productFromEvent = productsFromEvent.SingleOrDefault(pfe => pfe.Id == product.Id);
            //        if(productFromEvent is null)
            //        {
            //            continue;
            //        }
            //        if(product.HasQuantity && productFromEvent.Quantity is not null)
            //        {
            //            product.DecreaseQuantity((int)productFromEvent.Quantity);
            //        }
            //    }
            //    var auctionsToDelete = new List<Guid>();
            //    foreach(var auction in auctions)
            //    {
            //        var productFromEvent = productsFromEvent.SingleOrDefault(pfe => pfe.Id == auction.Id);
            //        if(productFromEvent is null)
            //        {
            //            continue;
            //        }
            //        if (productFromEvent.Quantity != null)
            //        {
            //            if(productFromEvent.Quantity == auction.Quantity)
            //            {
            //                auctionsToDelete.Add(auction.Id);
            //            }
            //            if(auction.HasQuantity && productFromEvent.Quantity is not null)
            //            {
            //                auction.DecreaseQuantity((int)productFromEvent.Quantity);
            //            }
            //        }

            //    }
            //    await _auctionRepository.UnlistManyAsync([.. auctionsToDelete]);
            //    await _productRepository.UpdateListedFlag([.. auctionsToDelete], false);
            //    await _productRepository.UpdateAsync();
            //}
        }
    }
}
