using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
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
    public sealed class ProductsListedHandler : IDomainEventHandler<ProductsListed>
    {
        private readonly IAuctionRepository _auctionRepository;

        public ProductsListedHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task HandleAsync(ProductsListed @event)
        {
            var products = @event.Products;
            var auctions = (await _auctionRepository.GetAllThatContainsInArrayAsync(products.Select(p => p.Id).ToArray())).ToList();
            if (auctions.Count != 0)
            {
                throw new ProductsCannotBeRelistedException(auctions.Select(a => a.Id).ToArray());
            }
            foreach (var product in products)
            {
                auctions.Add(new Auction(
                    id: product.Id,
                    sku: product.SKU,
                    name: product.Name,
                    price: product.Price,
                    description: product.Description,
                    imagePathUrls: product.Images.Select(i => i.ImageUrlPath).ToList(),
                    category: product.Category?.Name!,
                    quantity: product.Quantity,
                    additionalDescription: product.AdditionalDescription,
                    parameters: product.ProductParameters?.Select(pp => new AuctionParameter(pp.Parameter.Name, pp.Value)).ToList(),
                    manufacturer: product.Manufacturer?.Name
                ));
            }
            await _auctionRepository.ListManyAsync(auctions);
        }
    }
}
