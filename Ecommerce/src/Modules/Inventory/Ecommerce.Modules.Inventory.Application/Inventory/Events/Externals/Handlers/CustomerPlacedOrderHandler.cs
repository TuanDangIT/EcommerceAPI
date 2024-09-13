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
            IEnumerable<Product>? productsFromEvent = JsonSerializer.Deserialize<IEnumerable<Product>>(JsonSerializer.Serialize(@event.Products));
            if(productsFromEvent is null || !productsFromEvent.Any())
            {
                return;
            }
            productsFromEvent = productsFromEvent?.Where(pfe => pfe.Quantity is not null);
            if(productsFromEvent is not null)
            {
                var products = await _productRepository.GetAllThatContainsInArrayAsync(productsFromEvent.Select(pfe => pfe.Id).ToArray());
                var auctions = await _auctionRepository.GetAllThatContainsInArrayAsync(productsFromEvent.Select(pfe => pfe.Id).ToArray());
                foreach (var product in products)
                {
                    var productFromEvent = productsFromEvent.SingleOrDefault(pfe => pfe.Id == product.Id);
                    if(productFromEvent is null)
                    {
                        continue;
                    }
                    if(product.HasQuantity && productFromEvent.Quantity is not null)
                    {
                        product.DecreaseQuantity((int)productFromEvent.Quantity);
                    }
                }
                var auctionsToDelete = new List<Guid>();
                foreach(var auction in auctions)
                {
                    var productFromEvent = productsFromEvent.SingleOrDefault(pfe => pfe.Id == auction.Id);
                    if(productFromEvent is null)
                    {
                        continue;
                    }
                    if (productFromEvent.Quantity != null)
                    {
                        if(productFromEvent.Quantity == auction.Quantity)
                        {
                            auctionsToDelete.Add(auction.Id);
                        }
                        if(auction.HasQuantity && productFromEvent.Quantity is not null)
                        {
                            auction.DecreaseQuantity((int)productFromEvent.Quantity);
                        }
                    }

                }
                await _auctionRepository.DeleteManyAsync(auctionsToDelete.ToArray());
                await _productRepository.UpdateAsync();
            }
        }
    }
}
